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

                {"abstractMethod", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"annotation", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={"ScssMixinReferenceFormat"}, Html={""}, Xml={""}, Xaml={""} } },
                {"background", new VsSettingsModel { CCsharp={ "Plain Text", "Line Number"}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"bracket", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={"HTML Tag Delimiter"}, Xml={"XML Delimiter"}, Xaml={ "XAML Delimiter" } } },
                {"class", new VsSettingsModel { CCsharp={"class name","struct name", "module name"}, Cpp={""}, CssScss={"ScssMixinDeclarationFormat"}, Html={""}, Xml={"XML Name"}, Xaml={ "XAML Name" } } },
                {"commentTaskTag", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"constant", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={"CSS Keyword"}, Html={""}, Xml={""}, Xaml={""} } },
                {"currentLine", new VsSettingsModel { CCsharp={"CurrentLineActiveFormat"}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"deletionIndication", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"deprecatedMember", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"enum", new VsSettingsModel { CCsharp={"enum name"}, Cpp={"CppEnumSemanticTokenFormat"}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"field", new VsSettingsModel { CCsharp={""}, Cpp={"CppMemberFieldSemanticTokenFormat"}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"filteredSearchResultIndication", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"findScope", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"foreground", new VsSettingsModel { CCsharp={ "Plain Text","Identifier","delegate name"}, Cpp={""}, CssScss={"CSS Property Value", "ScssVariableDeclarationClassificationFormat"}, Html={ "HTML Attribute Value","HTML Entity" }, Xml={"XML Attribute Value","XML Attribute Quotes"}, Xaml={ "XAML Attribute Value", "XAML Attribute Quotes", "XAML Markup Extension Parameter Name", "XAML Markup Extension Parameter Value", "XAML Text" } } },
                {"inheritedMethod", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"interface", new VsSettingsModel { CCsharp={"interface name"}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"javadoc", new VsSettingsModel { CCsharp={"Breakpoint (Enabled)"}, Cpp={""}, CssScss={""}, Html={""}, Xml={"XML Doc Comment","xml doc comment - text", "xml doc comment - name","xml doc comment - processing instruction","xml doc comment - entity reference","xml doc comment - delimiter","xml doc comment - comment", "xml doc comment - attribute value", "xml doc comment - attribute quotes", "xml doc comment - attribute name"}, Xaml={""} } },
                {"javadocKeyword", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={ "" }, Xaml={""} } },
                {"javadocLink", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"javadocTag", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={"XML Doc Tag", "xml doc comment - cdata section"}, Xaml={""} } },
                {"keyword", new VsSettingsModel { CCsharp={"Keyword","Preprocessor Keyword"}, Cpp={"CppNamespaceSemanticTokenFormat","CppMacroSemanticTokenFormat"}, CssScss={"CSS Selector"}, Html={ "HTML Element Name","HTML Server-Side Script" }, Xml={""}, Xaml={ "" } } },
                {"lineNumber", new VsSettingsModel { CCsharp={"Line Number"}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"localVariable", new VsSettingsModel { CCsharp={""}, Cpp={"CppLocalVariableSemanticTokenFormat", "CppGlobalVariableSemanticTokenFormat"}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"localVariableDeclaration", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"method", new VsSettingsModel { CCsharp={""}, Cpp={"CppFunctionSemanticTokenFormat","CppMemberFunctionSemanticTokenFormat"}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"methodDeclaration", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"multiLineComment", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"number", new VsSettingsModel { CCsharp={"Number"}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={ "" } } },
                {"occurrenceIndication", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"operator", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={"HTML Operator"}, Xml={""}, Xaml={""} } },
                {"parameterVariable", new VsSettingsModel { CCsharp={""}, Cpp={"CppParameterSemanticTokenFormat"}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"searchResultIndication", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"selectionBackground", new VsSettingsModel { CCsharp={"Selected Text"}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"selectionForeground", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"singleLineComment", new VsSettingsModel { CCsharp={"Comment"}, Cpp={""}, CssScss={"CSS Comment"}, Html={"HTML Comment"}, Xml={"XML Comment", "XML CData Section"}, Xaml={ "XAML Comment", "XAML CData Section" } } },
                {"sourceHoverBackground", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"staticField", new VsSettingsModel { CCsharp={""}, Cpp={"CppStaticMemberFieldSemanticTokenFormat"}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"staticFinalField", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"staticMethod", new VsSettingsModel { CCsharp={""}, Cpp={"CppStaticMemberFunctionSemanticTokenFormat"}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"string", new VsSettingsModel { CCsharp={ "String","urlformat", "string - verbatim", "Current Statement" }, Cpp={""}, CssScss={"CSS String Value"}, Html={""}, Xml={"XML Text"}, Xaml={ "" } } },
                {"typeArgument", new VsSettingsModel { CCsharp={""}, Cpp={"CppTypeSemanticTokenFormat"}, CssScss={"CSS Property Name", "ScssVariableReferenceFormat"}, Html={"HTML Attribute"}, Xml={"XML Attribute"}, Xaml={ "XAML Attribute", "XAML Markup Extension Class" } } },
                {"typeParameter", new VsSettingsModel { CCsharp={"type parameter name"}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } },
                {"writeOccurrenceIndication", new VsSettingsModel { CCsharp={""}, Cpp={""}, CssScss={""}, Html={""}, Xml={""}, Xaml={""} } }

            };
        }
        #endregion populating the dictionary


    }
}
