// Skipping function DownloadNews(none), it contains poisonous unsupported syntaxes

// Skipping function ReadFile(none, none), it contains poisonous unsupported syntaxes

// Skipping function RenameFiles(none, none), it contains poisonous unsupported syntaxes

// Skipping function ListCollections(none, none), it contains poisonous unsupported syntaxes

// Skipping function TreeDirectory(none, none, none), it contains poisonous unsupported syntaxes

// Skipping function ReadCollection(none, none), it contains poisonous unsupported syntaxes

// Skipping function ReviewNoDownload(none, none), it contains poisonous unsupported syntaxes

// Skipping function ProcessJSON(none, none), it contains poisonous unsupported syntaxes

// Skipping function ProcessDownloadMega(none, none), it contains poisonous unsupported syntaxes

// Skipping function ProcessDownloadMediaFire(none, none), it contains poisonous unsupported syntaxes

// Skipping function CheckDirectoriesAndFiles(none), it contains poisonous unsupported syntaxes

// Skipping function RenameFile(none, none), it contains poisonous unsupported syntaxes

// Skipping function ExtractFile(none, none, none), it contains poisonous unsupported syntaxes

// Skipping function CreateFileReportNoDownload(none, none), it contains poisonous unsupported syntaxes

// Skipping function CreateFileReportNoRename(none, none), it contains poisonous unsupported syntaxes

// Skipping function CreateFileReportNews(none, none), it contains poisonous unsupported syntaxes

// Skipping function CreateFileLastDownload(none, none), it contains poisonous unsupported syntaxes

// Skipping function CreateFileReportCollections(none, none), it contains poisonous unsupported syntaxes

// Skipping function ReadLastDownload(none), it contains poisonous unsupported syntaxes

// Skipping function CreateFileReportCollection(none, none, none), it contains poisonous unsupported syntaxes

func @_DComics.Services.FormatSize$long$(none) -> none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :575 :8) {
^entry (%_bytes : none):
%0 = cbde.alloca none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :575 :34)
cbde.store %_bytes, %0 : memref<none> loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :575 :34)
br ^0

^0: // SimpleBlock
%1 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :577 :29) // Not a variable of known type: bytes
br ^1

^1: // BinaryBranchBlock
// Entity from another assembly: Math
%3 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :578 :30) // Not a variable of known type: number
%4 = constant 1024 : i32 loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :578 :39)
%5 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :578 :30) // Binary expression on unsupported types number / 1024
%6 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :578 :19) // Math.Round(number / 1024) (InvocationExpression)
%7 = constant 1 : i32 loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :578 :48)
%8 = cbde.unknown : i1  loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :578 :19) // comparison of unknown type: Math.Round(number / 1024) >= 1
cond_br %8, ^2, ^3 loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :578 :19)

^2: // SimpleBlock
%9 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :579 :16) // Not a variable of known type: number
%10 = constant 1024 : i32 loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :579 :26)
%11 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :579 :16) // Binary expression on unsupported types number /= 1024
br ^1

^3: // JumpBlock
%12 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :580 :19) // string (PredefinedType)
%13 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :580 :33) // "{0:n2}" (StringLiteralExpression)
%14 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :580 :43) // Not a variable of known type: number
%15 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :580 :19) // string.Format("{0:n2}", number) (InvocationExpression)
return %15 : none loc("I:\\DComics\\ComicsIDownload\\Services\\Services.cs" :580 :12)

^4: // ExitBlock
cbde.unreachable

}
