using IDE.Themes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// Helper class creating a mapping between VS and Eclipse theme variables. Helper for 
/// ThemeConverter. In essence, a one to many, Eclipse -> VS, with VsSettingsModel helper class.
/// </summary>


namespace IDE.Themes.Services {


    public class ThemeDictionary {

        /*PROPERTIES*/

        #region dictionary mapping

        public IDictionary<String, VsSettingsModel> Mapping { get; set; }

        //each value in a map should only be used once. Fills in empty (default) CCsharp values: default CCsharp mapping
        public IDictionary<String, String> DefaultValues { get; set; } = new Dictionary<String, String> {

            {"\"annotation\"","\"Comment\"" },
            {"\"bracket\"","\"Identifier\"" },
            {"\"constant\"","\"Identifier\"" },
            {"\"field\"","\"Identifier\"" },
            {"\"javadocTag\"","\"xml doc comment - text\"" },
            {"\"localVariable\"","\"Identifier\"" },
            {"\"method\"","\"Identifier\"" },
            {"\"operator\"","\"Identifier\"" },
            {"\"parameterVariable\"","\"Identifier\"" },
            {"\"staticField\"","\"Identifier\"" },
            {"\"staticMethod\"","\"Identifier\"" },
            {"\"typeArgument\"","\"interface name\"" },
            { "\"javadocLink\"", "\"xml doc comment - text\"" },
            { "\"javadocKeyword\"", "\"xml doc comment - text\"" },
            { "\"multiLineComment\"", "\"Comment\"" },
            { "\"selectionForeground\"", "\"Identifier\"" },
            { "\"localVariableDeclaration\"", "\"Identifier\"" },
            { "\"deletionIndication\"", "\"type parameter name\"" },
            { "\"deprecatedMember\"", "\"type parameter name\"" },
            { "\"methodDeclaration\"", "\"Identifier\"" },
            { "\"inheritedMethod\"", "\"Identifier\"" },
            { "\"abstractMethod\"", "\"Identifier\"" },
            { "\"staticFinalField\"", "\"Identifier\"" },
            { "\"commentTaskTag\"", "\"Comment\"" },
            { "\"currentLine\"", "\"Plain Text\"" }
        };

        //some Eclipse keys have no values, so their value is the same as a corresponding key with a value
        public IDictionary<String, String> EmptyValues { get; set; } = new Dictionary<String, String> {

            { "\"javadocLink\"", "\"javadoc\"" },
            { "\"javadocKeyword\"", "\"javadoc\"" },
            { "\"multiLineComment\"", "\"singleLineComment\"" },
            { "\"selectionForeground\"", "\"foreground\"" },
            { "\"localVariableDeclaration\"", "\"foreground\"" },
            { "\"deletionIndication\"", "\"typeParameter\"" },
            { "\"deprecatedMember\"", "\"typeParameter\"" },
            { "\"methodDeclaration\"", "\"method\"" },
            { "\"inheritedMethod\"", "\"method\"" },
            { "\"abstractMethod\"", "\"method\"" },
            { "\"staticFinalField\"", "\"staticField\"" },
            { "\"commentTaskTag\"", "\"singleLineComment\"" }     
        };

        //mapping xml items to the corresponding epf lines
        public IDictionary<String, String> EpfValues { get; set; } = new Dictionary<String, String>() {

            {"searchResultIndication", "searchResultIndicationColor=" },
            {"filteredSearchResultIndication", "filteredSearchResultIndicationColor="},
            {"occurrenceIndication", "occurrenceIndicationColor="},
            {"writeOccurrenceIndication", "writeOccurrenceIndicationColor="},
            {"findScope", "AbstractTextEditor.Color.FindScope="},
            {"deletionIndication", "deletionIndicationColor="},
            {"sourceHoverBackground", "sourceHoverBackgroundColor="},
            {"singleLineComment", "java_single_line_comment="},
            {"multiLineComment", "java_multi_line_comment="},
            {"commentTaskTag", "java_comment_task_tag="},
            {"javadoc", "java_doc_default="},
            {"javadocLink", "java_doc_link="},
            {"javadocTag", "java_doc_tag="},
            {"javadocKeyword", "java_doc_keyword="},
            {"class", "semanticHighlighting.class.color="},
            {"interface", "semanticHighlighting.interface.color="},
            {"method", "semanticHighlighting.method.color="},
            {"methodDeclaration", "semanticHighlighting.methodDeclarationName.color="},
            {"bracket", "java_bracket="},
            {"number", "semanticHighlighting.number.color="},
            {"string", "java_string="},
            {"operator", "java_operator="},
            {"keyword", "java_keyword="},
            {"annotation", "semanticHighlighting.annotation.color="},
            {"staticMethod", "semanticHighlighting.staticMethodInvocation.color="},
            {"localVariable", "semanticHighlighting.localVariable.color="},
            {"localVariableDeclaration", "semanticHighlighting.localVariableDeclaration.color="},
            {"field", "semanticHighlighting.field.color="},
            {"staticField", "semanticHighlighting.staticField.color="},
            {"staticFinalField", "semanticHighlighting.staticFinalField.color="},
            {"deprecatedMember", "semanticHighlighting.deprecatedMember.color="},
            {"enum", "semanticHighlighting.enum.color="},
            {"inheritedMethod", "semanticHighlighting.inheritedMethodInvocation.color="},
            {"abstractMethod", "semanticHighlighting.abstractMethodInvocation.color="},
            {"parameterVariable", "semanticHighlighting.parameterVariable.color="},
            {"typeArgument", "semanticHighlighting.typeArgument.color="},
            {"typeParameter", "semanticHighlighting.typeParameter.color="},
            {"constant", "/instance/com.powerflasher.fdt.ui/AS_CONSTANTS_color="},
            {"foreground", "AbstractTextEditor.Color.Foreground="},
            {"background", "AbstractTextEditor.Color.Background="},
            {"currentLine", "currentLineColor="},
            {"lineNumber", "lineNumberColor="},
            {"selectionBackground", "AbstractTextEditor.Color.SelectionBackground="},
            {"selectionForeground", "AbstractTextEditor.Color.SelectionForeground="}
        };

        #endregion dictionary mapping

        /*CONSTRUCTOR*/

        #region constructor, populate on initialization

        public ThemeDictionary() {

            PopulateMap();
        }
        #endregion constructor, populate on initialization

        /*METHODS*/

        #region populating the dictionary
        //populates the hashmap 
        private void PopulateMap() {

            Mapping = new Dictionary<String, VsSettingsModel> {

                {"\"searchResultIndication\"", new VsSettingsModel { CCsharp=new List<string>{ "\"MarkerFormatDefinition/HighlightedReference\"", "\"MarkerFormatDefinition/HighlightedWrittenReference\"", "\"MarkerFormatDefinition/HighlightParameterFormatDefinition\"" }, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"filteredSearchResultIndication\"", new VsSettingsModel { CCsharp=new List<string>{"\"MarkerFormatDefinition/HighlightedReference\"", "\"MarkerFormatDefinition/HighlightedWrittenReference\"", "\"MarkerFormatDefinition/HighlightParameterFormatDefinition\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"occurrenceIndication\"", new VsSettingsModel { CCsharp=new List<string>{"\"MarkerFormatDefinition/HighlightedReference\"", "\"MarkerFormatDefinition/HighlightedWrittenReference\"", "\"MarkerFormatDefinition/HighlightParameterFormatDefinition\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"writeOccurrenceIndication\"", new VsSettingsModel { CCsharp=new List<string>{"\"MarkerFormatDefinition/HighlightedReference\"", "\"MarkerFormatDefinition/HighlightedWrittenReference\"", "\"MarkerFormatDefinition/HighlightParameterFormatDefinition\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"findScope\"", new VsSettingsModel { CCsharp=new List<string>{"\"MarkerFormatDefinition/FindHighlight\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"deletionIndication\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"sourceHoverBackground\"", new VsSettingsModel { CCsharp=new List<string>{"\"MarkerFormatDefinition/HighlightedReference\"", "\"MarkerFormatDefinition/HighlightedWrittenReference\"", "\"MarkerFormatDefinition/HighlightParameterFormatDefinition\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"singleLineComment\"", new VsSettingsModel { CCsharp=new List<string>{"\"Comment\""}, Cpp=new List<string>{""}, CssScss=new List<string>{"\"CSS Comment\""}, Html=new List<string>{"\"HTML Comment\""}, Xml=new List<string>{ "\"XML Comment\"", "\"XML CData Section\""}, Xaml=new List<string>{ "\"XAML Comment\"", "\"XAML CData Section\"" } } },
                {"\"multiLineComment\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"commentTaskTag\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"javadoc\"", new VsSettingsModel { CCsharp=new List<string>{"\"xml doc comment - text\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{ "\"XML Doc Comment\"", "\"xml doc comment - text\"", "\"xml doc comment - name\"", "\"xml doc comment - processing instruction\"", "\"xml doc comment - entity reference\"", "\"xml doc comment - delimiter\"", "\"xml doc comment - comment\"", "\"xml doc comment - attribute value\"", "\"xml doc comment - attribute quotes\"", "\"xml doc comment - attribute name\""}, Xaml=new List<string>{""} } },
                {"\"javadocLink\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"javadocTag\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{ "\"XML Doc Tag\"", "\"xml doc comment - cdata section\""}, Xaml=new List<string>{""} } },
                {"\"javadocKeyword\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"class\"", new VsSettingsModel { CCsharp=new List<string>{ "\"class name\"", "\"struct name\"", "\"module name\""}, Cpp=new List<string>{""}, CssScss=new List<string>{"\"ScssMixinDeclarationFormat\""}, Html=new List<string>{""}, Xml=new List<string>{"\"XML Name\""}, Xaml=new List<string>{ "\"XAML Name\"" } } },
                {"\"interface\"", new VsSettingsModel { CCsharp=new List<string>{"\"interface name\"", "\"User Types(Interfaces)\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"method\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{ "\"CppFunctionSemanticTokenFormat\"", "\"CppMemberFunctionSemanticTokenFormat\""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"methodDeclaration\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"bracket\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{"\"HTML Tag Delimiter\""}, Xml=new List<string>{"\"XML Delimiter\""}, Xaml=new List<string>{ "\"XAML Delimiter\"" } } },
                {"\"number\"", new VsSettingsModel { CCsharp=new List<string>{"\"Number\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{ "" } } },
                {"\"string\"", new VsSettingsModel { CCsharp=new List<string>{ "\"String\"", "\"string - verbatim\"", "\"String(C# @ Verbatim)\"" }, Cpp=new List<string>{""}, CssScss=new List<string>{"\"CSS String Value\""}, Html=new List<string>{""}, Xml=new List<string>{"\"XML Text\""}, Xaml=new List<string>{ "" } } },
                {"\"operator\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{"\"HTML Operator\""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"keyword\"", new VsSettingsModel { CCsharp=new List<string>{ "\"Keyword\"", "\"Preprocessor Keyword\""}, Cpp=new List<string>{ "\"CppNamespaceSemanticTokenFormat\"", "\"CppMacroSemanticTokenFormat\""}, CssScss=new List<string>{"\"CSS Selector\""}, Html=new List<string>{ "\"HTML Element Name\"", "\"HTML Server-Side Script\"" }, Xml=new List<string>{""}, Xaml=new List<string>{ "" } } },
                {"\"annotation\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{"\"ScssMixinReferenceFormat\""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"staticMethod\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{"\"CppStaticMemberFunctionSemanticTokenFormat\""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"localVariable\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{ "\"CppLocalVariableSemanticTokenFormat\"", "\"CppGlobalVariableSemanticTokenFormat\""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"localVariableDeclaration\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"field\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{"\"CppMemberFieldSemanticTokenFormat\""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"staticField\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{"\"CppStaticMemberFieldSemanticTokenFormat\""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"staticFinalField\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"deprecatedMember\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"enum\"", new VsSettingsModel { CCsharp=new List<string>{"\"enum name\"", "\"User Types(Enums)\""}, Cpp=new List<string>{"\"CppEnumSemanticTokenFormat\""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"inheritedMethod\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"abstractMethod\"", new VsSettingsModel { CCsharp = new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"parameterVariable\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{"\"CppParameterSemanticTokenFormat\""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"typeArgument\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{"\"CppTypeSemanticTokenFormat\""}, CssScss=new List<string>{ "\"CSS Property Name\"", "\"ScssVariableReferenceFormat\""}, Html=new List<string>{"\"HTML Attribute Name\""}, Xml=new List<string>{"\"XML Attribute\""}, Xaml=new List<string>{ "\"XAML Attribute\"", "\"XAML Markup Extension Class\"" } } },
                {"\"typeParameter\"", new VsSettingsModel { CCsharp=new List<string>{"\"type parameter name\"", "\"User Types(Value types)\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"constant\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{"\"CSS Keyword\""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"foreground\"", new VsSettingsModel { CCsharp=new List<string>{ "\"Identifier\"", "\"delegate name\"", "\"User Types(Delegates)\""}, Cpp=new List<string>{""}, CssScss=new List<string>{ "\"CSS Property Value\"", "\"ScssVariableDeclarationClassificationFormat\""}, Html=new List<string>{ "\"HTML Attribute Value\"", "\"HTML Entity\"" }, Xml=new List<string>{ "\"XML Attribute Value\"", "\"XML Attribute Quotes\""}, Xaml=new List<string>{ "\"XAML Attribute Value\"", "\"XAML Attribute Quotes\"", "\"XAML Markup Extension Parameter Name\"", "\"XAML Markup Extension Parameter Value\"", "\"XAML Text\"" } } },
                {"\"background\"", new VsSettingsModel { CCsharp=new List<string>{ "\"Plain Text\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"currentLine\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"lineNumber\"", new VsSettingsModel { CCsharp=new List<string>{"\"Line Number\"", "\"Line Numbers\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"selectionBackground\"", new VsSettingsModel { CCsharp=new List<string>{"\"Selected Text\""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } },
                {"\"selectionForeground\"", new VsSettingsModel { CCsharp=new List<string>{""}, Cpp=new List<string>{""}, CssScss=new List<string>{""}, Html=new List<string>{""}, Xml=new List<string>{""}, Xaml=new List<string>{""} } }

            };
        }
        #endregion populating the dictionary

    }
}
