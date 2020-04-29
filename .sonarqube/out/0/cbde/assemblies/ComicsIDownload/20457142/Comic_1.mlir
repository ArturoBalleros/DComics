func @_DComics.Models.Comic.ToString$$() -> none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :24 :8) {
^entry :
br ^0

^0: // JumpBlock
%0 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :19) // string (PredefinedType)
%1 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :33) // "Id: {0}, Name: {1}, Link: {2}, NameWeb: {3}, SizeWeb: {4}" (StringLiteralExpression)
%2 = cbde.unknown : i32 loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :94) // Not a variable of known type: Id
%3 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :98) // Not a variable of known type: Name
%4 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :104) // Not a variable of known type: Link
%5 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :110) // Not a variable of known type: NameWeb
%6 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :119) // Not a variable of known type: SizeWeb
%7 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :19) // string.Format("Id: {0}, Name: {1}, Link: {2}, NameWeb: {3}, SizeWeb: {4}", Id, Name, Link, NameWeb, SizeWeb) (InvocationExpression)
return %7 : none loc("I:\\DComics\\ComicsIDownload\\Models\\Comic.cs" :26 :12)

^1: // ExitBlock
cbde.unreachable

}
// Skipping function Serializer(none, i1), it contains poisonous unsupported syntaxes

