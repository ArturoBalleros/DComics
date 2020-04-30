func @_ReadDCInfo.Program.Main$string$$$(none) -> () loc("I:\\DComics\\ReadDCInfo\\Program.cs" :7 :8) {
^entry (%_args : none):
%0 = cbde.alloca none loc("I:\\DComics\\ReadDCInfo\\Program.cs" :7 :25)
cbde.store %_args, %0 : memref<none> loc("I:\\DComics\\ReadDCInfo\\Program.cs" :7 :25)
br ^0

^0: // SimpleBlock
%1 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Program.cs" :9 :40) // new ScrapService() (ObjectCreationExpression)
%3 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Program.cs" :10 :12) // Not a variable of known type: scrapService
%4 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Program.cs" :10 :12) // scrapService.scrapReadDC() (InvocationExpression)
// Entity from another assembly: Console
%5 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Program.cs" :11 :30) // "Hello World!" (StringLiteralExpression)
%6 = cbde.unknown : none loc("I:\\DComics\\ReadDCInfo\\Program.cs" :11 :12) // Console.WriteLine("Hello World!") (InvocationExpression)
br ^1

^1: // ExitBlock
return

}
