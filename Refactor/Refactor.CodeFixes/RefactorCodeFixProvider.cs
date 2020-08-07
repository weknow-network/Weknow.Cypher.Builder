using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace Refactor
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RefactorCodeFixProvider)), Shared]
    public class RefactorCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(RefactorAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<SimpleLambdaExpressionSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedSolution: c => MakeUppercaseAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private async Task<Solution> MakeUppercaseAsync(Document document, SimpleLambdaExpressionSyntax lambda, CancellationToken cancellationToken)
        {
            var lambdas = new List<SimpleLambdaExpressionSyntax>();
            var lam = lambda;
            while (lam.Body is SimpleLambdaExpressionSyntax l)
            {
                lambdas.Add(lam);
                lam = lam.Body as SimpleLambdaExpressionSyntax;
            }
            lambdas.Add(lam);
            var newLambda = SyntaxFactory.ParenthesizedLambdaExpression(lam.Body);
            var varDec = SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.NullableType(
                    SyntaxFactory.IdentifierName("IVar")),
                    SyntaxFactory.SeparatedList(
                        lambdas.Select(l => SyntaxFactory.VariableDeclarator(
                            l.Parameter.Identifier).WithInitializer(
                            SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)))).ToArray())));
            SyntaxNode parent = lambda;
            while (!(parent is BlockSyntax))
            {
                parent = parent.Parent;
            }
            var block = parent as BlockSyntax;
            var newBlock = block.ReplaceNode(lambda, newLambda);
            newBlock = newBlock.WithStatements(newBlock.Statements.Insert(0, varDec));

            var root = await document.GetSyntaxRootAsync();

            var newRoot = root.ReplaceNode(block, newBlock);

            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument.Project.Solution;
        }
    }
}
