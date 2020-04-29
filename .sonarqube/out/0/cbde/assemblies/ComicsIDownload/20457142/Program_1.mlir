// Skipping function Main(none), it contains poisonous unsupported syntaxes

func @_DComics.Program.initLogger$$() -> none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :85 :8) {
^entry :
br ^0

^0: // JumpBlock
// Entity from another assembly: LogManager
// Entity from another assembly: Assembly
%0 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :96 :57) // Assembly.GetEntryAssembly() (InvocationExpression)
%1 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :96 :32) // LogManager.GetRepository(Assembly.GetEntryAssembly()) (InvocationExpression)
// Entity from another assembly: XmlConfigurator
%3 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :97 :38) // Not a variable of known type: logRepository
%4 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :97 :66) // "log4net.config" (StringLiteralExpression)
%5 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :97 :53) // new FileInfo("log4net.config") (ObjectCreationExpression)
%6 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :97 :12) // XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config")) (InvocationExpression)
// Entity from another assembly: LogManager
%7 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :98 :40) // typeof(Program) (TypeOfExpression)
%8 = cbde.unknown : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :98 :19) // LogManager.GetLogger(typeof(Program)) (InvocationExpression)
return %8 : none loc("I:\\DComics\\ComicsIDownload\\Program.cs" :98 :12)

^1: // ExitBlock
cbde.unreachable

}
