using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Data Model representing the User Color Table. Stores all the theme color variables.
/// </summary>

namespace IDE.Themes.Models {

    public class UserColorDataModel {

        //PROPERTIES: TABLE COLUMNS
        [Key]
        [MaxLength(50)]
        public string ColorId { get; set; }

        public DateTimeOffset Date { get; set; }

        [MaxLength(8)]
        public string SearchResultIndication { get; set; }
        [MaxLength(8)]
        public string FilteredSearchResultIndication { get; set; }
        [MaxLength(8)]
        public string OccurrenceIndication { get; set; }
        [MaxLength(8)]
        public string WriteOccurrenceIndication { get; set; }
        [MaxLength(8)]
        public string FindScope { get; set; }
        [MaxLength(8)]
        public string DeletionIndication { get; set; }
        [MaxLength(8)]
        public string SourceHoverBackground { get; set; }
        [MaxLength(8)]
        public string SingleLineComment { get; set; }
        [MaxLength(8)]
        public string MultiLineComment { get; set; }
        [MaxLength(8)]
        public string CommentTaskTag { get; set; }
        [MaxLength(8)]
        public string Javadoc { get; set; }
        [MaxLength(8)]
        public string JavadocLink { get; set; }
        [MaxLength(8)]
        public string JavadocTag { get; set; }
        [MaxLength(8)]
        public string JavadocKeyword { get; set; }
        [MaxLength(8)]
        public string Class { get; set; }
        [MaxLength(8)]
        public string Interface { get; set; }
        [MaxLength(8)]
        public string Method { get; set; }
        [MaxLength(8)]
        public string MethodDeclaration { get; set; }
        [MaxLength(8)]
        public string Bracket { get; set; }
        [MaxLength(8)]
        public string Number { get; set; }
        [MaxLength(8)]
        public string String { get; set; }
        [MaxLength(8)]
        public string Operator { get; set; }
        [MaxLength(8)]
        public string Keyword { get; set; }
        [MaxLength(8)]
        public string Annotation { get; set; }
        [MaxLength(8)]
        public string StaticMethod { get; set; }
        [MaxLength(8)]
        public string LocalVariable { get; set; }
        [MaxLength(8)]
        public string LocalVariableDeclaration { get; set; }
        [MaxLength(8)]
        public string Field { get; set; }
        [MaxLength(8)]
        public string StaticField { get; set; }
        [MaxLength(8)]
        public string StaticFinalField { get; set; }
        [MaxLength(8)]
        public string DeprecatedMember { get; set; }
        [MaxLength(8)]
        public string Enum { get; set; }
        [MaxLength(8)]
        public string InheritedMethod { get; set; }
        [MaxLength(8)]
        public string AbstractMethod { get; set; }
        [MaxLength(8)]
        public string ParameterVariable { get; set; }
        [MaxLength(8)]
        public string TypeArgument { get; set; }
        [MaxLength(8)]
        public string TypeParameter { get; set; }
        [MaxLength(8)]
        public string Constant { get; set; }
        [MaxLength(8)]
        public string Background { get; set; }
        [MaxLength(8)]
        public string CurrentLine { get; set; }
        [MaxLength(8)]
        public string Foreground { get; set; }
        [MaxLength(8)]
        public string LineNumber { get; set; }
        [MaxLength(8)]
        public string SelectionBackground { get; set; }
        [MaxLength(8)]
        public string SelectionForeground { get; set; }

    }
}
