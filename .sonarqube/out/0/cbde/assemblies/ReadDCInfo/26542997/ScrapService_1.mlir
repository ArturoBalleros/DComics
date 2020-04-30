func @_ReadDCInfo.Services.ScrapService.scrapReadDC$$() -> () loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :15 :8) {
^entry :
br ^0

^0: // SimpleBlock
// Skipped because MethodDeclarationSyntax or ClassDeclarationSyntax or NamespaceDeclarationSyntax: GetIssues
%0 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :27 :22) // @"https://www.readdc.com/Action-Comics-2016/comics-series/71637?ref=YnJvd3NlL3Nlcmllcy9kZXNrdG9wL2xpc3Qvc2VyaWVzTGlzdA&Issues_pg=" (StringLiteralExpression)
%1 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :27 :12) // GetIssues(@"https://www.readdc.com/Action-Comics-2016/comics-series/71637?ref=YnJvd3NlL3Nlcmllcy9kZXNrdG9wL2xpc3Qvc2VyaWVzTGlzdA&Issues_pg=") (InvocationExpression)
br ^1

^1: // ExitBlock
return

}
func @_ReadDCInfo.Services.ScrapService.GetListUrlFilters$$() -> none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :31 :8) {
^entry :
br ^0

^0: // JumpBlock
%0 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :33 :36) // new List<string>() (ObjectCreationExpression)
%2 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :35 :25) // @"https://www.readdc.com/browse-series?seriesList_alpha={0}&seriesList_pg=" (StringLiteralExpression)
%4 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :36 :12) // Not a variable of known type: listUrls
%5 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :36 :25) // string (PredefinedType)
%6 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :36 :39) // Not a variable of known type: url
%7 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :36 :44) // "%2523" (StringLiteralExpression)
%8 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :36 :25) // string.Format(url, "%2523") (InvocationExpression)
%9 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :36 :12) // listUrls.Add(string.Format(url, "%2523")) (InvocationExpression)
%10 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :37 :12) // Not a variable of known type: listUrls
%11 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :37 :25) // string (PredefinedType)
%12 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :37 :39) // Not a variable of known type: url
%13 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :37 :44) // "A" (StringLiteralExpression)
%14 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :37 :25) // string.Format(url, "A") (InvocationExpression)
%15 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :37 :12) // listUrls.Add(string.Format(url, "A")) (InvocationExpression)
%16 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :38 :12) // Not a variable of known type: listUrls
%17 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :38 :25) // string (PredefinedType)
%18 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :38 :39) // Not a variable of known type: url
%19 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :38 :44) // "B" (StringLiteralExpression)
%20 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :38 :25) // string.Format(url, "B") (InvocationExpression)
%21 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :38 :12) // listUrls.Add(string.Format(url, "B")) (InvocationExpression)
%22 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :39 :12) // Not a variable of known type: listUrls
%23 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :39 :25) // string (PredefinedType)
%24 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :39 :39) // Not a variable of known type: url
%25 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :39 :44) // "C" (StringLiteralExpression)
%26 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :39 :25) // string.Format(url, "C") (InvocationExpression)
%27 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :39 :12) // listUrls.Add(string.Format(url, "C")) (InvocationExpression)
%28 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :40 :12) // Not a variable of known type: listUrls
%29 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :40 :25) // string (PredefinedType)
%30 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :40 :39) // Not a variable of known type: url
%31 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :40 :44) // "D" (StringLiteralExpression)
%32 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :40 :25) // string.Format(url, "D") (InvocationExpression)
%33 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :40 :12) // listUrls.Add(string.Format(url, "D")) (InvocationExpression)
%34 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :41 :12) // Not a variable of known type: listUrls
%35 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :41 :25) // string (PredefinedType)
%36 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :41 :39) // Not a variable of known type: url
%37 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :41 :44) // "E" (StringLiteralExpression)
%38 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :41 :25) // string.Format(url, "E") (InvocationExpression)
%39 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :41 :12) // listUrls.Add(string.Format(url, "E")) (InvocationExpression)
%40 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :42 :12) // Not a variable of known type: listUrls
%41 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :42 :25) // string (PredefinedType)
%42 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :42 :39) // Not a variable of known type: url
%43 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :42 :44) // "F" (StringLiteralExpression)
%44 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :42 :25) // string.Format(url, "F") (InvocationExpression)
%45 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :42 :12) // listUrls.Add(string.Format(url, "F")) (InvocationExpression)
%46 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :43 :12) // Not a variable of known type: listUrls
%47 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :43 :25) // string (PredefinedType)
%48 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :43 :39) // Not a variable of known type: url
%49 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :43 :44) // "G" (StringLiteralExpression)
%50 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :43 :25) // string.Format(url, "G") (InvocationExpression)
%51 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :43 :12) // listUrls.Add(string.Format(url, "G")) (InvocationExpression)
%52 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :44 :12) // Not a variable of known type: listUrls
%53 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :44 :25) // string (PredefinedType)
%54 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :44 :39) // Not a variable of known type: url
%55 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :44 :44) // "H" (StringLiteralExpression)
%56 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :44 :25) // string.Format(url, "H") (InvocationExpression)
%57 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :44 :12) // listUrls.Add(string.Format(url, "H")) (InvocationExpression)
%58 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :45 :12) // Not a variable of known type: listUrls
%59 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :45 :25) // string (PredefinedType)
%60 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :45 :39) // Not a variable of known type: url
%61 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :45 :44) // "I" (StringLiteralExpression)
%62 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :45 :25) // string.Format(url, "I") (InvocationExpression)
%63 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :45 :12) // listUrls.Add(string.Format(url, "I")) (InvocationExpression)
%64 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :46 :12) // Not a variable of known type: listUrls
%65 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :46 :25) // string (PredefinedType)
%66 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :46 :39) // Not a variable of known type: url
%67 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :46 :44) // "J" (StringLiteralExpression)
%68 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :46 :25) // string.Format(url, "J") (InvocationExpression)
%69 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :46 :12) // listUrls.Add(string.Format(url, "J")) (InvocationExpression)
%70 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :47 :12) // Not a variable of known type: listUrls
%71 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :47 :25) // string (PredefinedType)
%72 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :47 :39) // Not a variable of known type: url
%73 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :47 :44) // "K" (StringLiteralExpression)
%74 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :47 :25) // string.Format(url, "K") (InvocationExpression)
%75 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :47 :12) // listUrls.Add(string.Format(url, "K")) (InvocationExpression)
%76 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :48 :12) // Not a variable of known type: listUrls
%77 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :48 :25) // string (PredefinedType)
%78 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :48 :39) // Not a variable of known type: url
%79 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :48 :44) // "L" (StringLiteralExpression)
%80 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :48 :25) // string.Format(url, "L") (InvocationExpression)
%81 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :48 :12) // listUrls.Add(string.Format(url, "L")) (InvocationExpression)
%82 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :49 :12) // Not a variable of known type: listUrls
%83 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :49 :25) // string (PredefinedType)
%84 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :49 :39) // Not a variable of known type: url
%85 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :49 :44) // "M" (StringLiteralExpression)
%86 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :49 :25) // string.Format(url, "M") (InvocationExpression)
%87 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :49 :12) // listUrls.Add(string.Format(url, "M")) (InvocationExpression)
%88 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :50 :12) // Not a variable of known type: listUrls
%89 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :50 :25) // string (PredefinedType)
%90 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :50 :39) // Not a variable of known type: url
%91 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :50 :44) // "N" (StringLiteralExpression)
%92 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :50 :25) // string.Format(url, "N") (InvocationExpression)
%93 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :50 :12) // listUrls.Add(string.Format(url, "N")) (InvocationExpression)
%94 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :51 :12) // Not a variable of known type: listUrls
%95 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :51 :25) // string (PredefinedType)
%96 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :51 :39) // Not a variable of known type: url
%97 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :51 :44) // "O" (StringLiteralExpression)
%98 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :51 :25) // string.Format(url, "O") (InvocationExpression)
%99 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :51 :12) // listUrls.Add(string.Format(url, "O")) (InvocationExpression)
%100 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :52 :12) // Not a variable of known type: listUrls
%101 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :52 :25) // string (PredefinedType)
%102 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :52 :39) // Not a variable of known type: url
%103 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :52 :44) // "P" (StringLiteralExpression)
%104 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :52 :25) // string.Format(url, "P") (InvocationExpression)
%105 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :52 :12) // listUrls.Add(string.Format(url, "P")) (InvocationExpression)
%106 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :53 :12) // Not a variable of known type: listUrls
%107 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :53 :25) // string (PredefinedType)
%108 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :53 :39) // Not a variable of known type: url
%109 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :53 :44) // "Q" (StringLiteralExpression)
%110 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :53 :25) // string.Format(url, "Q") (InvocationExpression)
%111 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :53 :12) // listUrls.Add(string.Format(url, "Q")) (InvocationExpression)
%112 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :54 :12) // Not a variable of known type: listUrls
%113 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :54 :25) // string (PredefinedType)
%114 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :54 :39) // Not a variable of known type: url
%115 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :54 :44) // "R" (StringLiteralExpression)
%116 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :54 :25) // string.Format(url, "R") (InvocationExpression)
%117 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :54 :12) // listUrls.Add(string.Format(url, "R")) (InvocationExpression)
%118 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :55 :12) // Not a variable of known type: listUrls
%119 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :55 :25) // string (PredefinedType)
%120 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :55 :39) // Not a variable of known type: url
%121 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :55 :44) // "S" (StringLiteralExpression)
%122 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :55 :25) // string.Format(url, "S") (InvocationExpression)
%123 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :55 :12) // listUrls.Add(string.Format(url, "S")) (InvocationExpression)
%124 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :56 :12) // Not a variable of known type: listUrls
%125 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :56 :25) // string (PredefinedType)
%126 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :56 :39) // Not a variable of known type: url
%127 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :56 :44) // "T" (StringLiteralExpression)
%128 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :56 :25) // string.Format(url, "T") (InvocationExpression)
%129 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :56 :12) // listUrls.Add(string.Format(url, "T")) (InvocationExpression)
%130 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :57 :12) // Not a variable of known type: listUrls
%131 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :57 :25) // string (PredefinedType)
%132 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :57 :39) // Not a variable of known type: url
%133 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :57 :44) // "U" (StringLiteralExpression)
%134 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :57 :25) // string.Format(url, "U") (InvocationExpression)
%135 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :57 :12) // listUrls.Add(string.Format(url, "U")) (InvocationExpression)
%136 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :58 :12) // Not a variable of known type: listUrls
%137 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :58 :25) // string (PredefinedType)
%138 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :58 :39) // Not a variable of known type: url
%139 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :58 :44) // "V" (StringLiteralExpression)
%140 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :58 :25) // string.Format(url, "V") (InvocationExpression)
%141 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :58 :12) // listUrls.Add(string.Format(url, "V")) (InvocationExpression)
%142 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :59 :12) // Not a variable of known type: listUrls
%143 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :59 :25) // string (PredefinedType)
%144 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :59 :39) // Not a variable of known type: url
%145 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :59 :44) // "W" (StringLiteralExpression)
%146 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :59 :25) // string.Format(url, "W") (InvocationExpression)
%147 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :59 :12) // listUrls.Add(string.Format(url, "W")) (InvocationExpression)
%148 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :60 :12) // Not a variable of known type: listUrls
%149 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :60 :25) // string (PredefinedType)
%150 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :60 :39) // Not a variable of known type: url
%151 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :60 :44) // "X" (StringLiteralExpression)
%152 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :60 :25) // string.Format(url, "X") (InvocationExpression)
%153 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :60 :12) // listUrls.Add(string.Format(url, "X")) (InvocationExpression)
%154 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :61 :12) // Not a variable of known type: listUrls
%155 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :61 :25) // string (PredefinedType)
%156 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :61 :39) // Not a variable of known type: url
%157 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :61 :44) // "Y" (StringLiteralExpression)
%158 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :61 :25) // string.Format(url, "Y") (InvocationExpression)
%159 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :61 :12) // listUrls.Add(string.Format(url, "Y")) (InvocationExpression)
%160 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :62 :12) // Not a variable of known type: listUrls
%161 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :62 :25) // string (PredefinedType)
%162 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :62 :39) // Not a variable of known type: url
%163 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :62 :44) // "Z" (StringLiteralExpression)
%164 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :62 :25) // string.Format(url, "Z") (InvocationExpression)
%165 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :62 :12) // listUrls.Add(string.Format(url, "Z")) (InvocationExpression)
%166 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :80 :19) // Not a variable of known type: listUrls
return %166 : none loc("I:\\DComics\\ReadDCInfo\\Services\\ScrapService.cs" :80 :12)

^1: // ExitBlock
cbde.unreachable

}
// Skipping function GetListUrlCollections(none), it contains poisonous unsupported syntaxes

// Skipping function GetIssues(none), it contains poisonous unsupported syntaxes

// Skipping function GetDataIssue(none), it contains poisonous unsupported syntaxes

// Skipping function DownloadImage(none, none), it contains poisonous unsupported syntaxes

