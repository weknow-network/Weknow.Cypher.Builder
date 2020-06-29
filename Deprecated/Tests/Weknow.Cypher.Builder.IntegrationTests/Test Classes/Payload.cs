using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// https://neo4j.com/docs/cypher-refcard/current/

namespace Weknow.CoreIntegrationTests
{
    public class Payload : IEquatable<Payload>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }

        public string Description { get; set; }

        #region Equality Pattern

        public override bool Equals(object obj)
        {
            return Equals(obj as Payload);
        }

        public bool Equals([AllowNull] Payload other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Date == other.Date &&
                   Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Date, Description);
        }

        public static bool operator ==(Payload left, Payload right)
        {
            return EqualityComparer<Payload>.Default.Equals(left, right);
        }

        public static bool operator !=(Payload left, Payload right)
        {
            return !(left == right);
        }


        #endregion // Equality Pattern
    }
}