using System.Reflection;
using System.Runtime.CompilerServices;

//
// Allgemeine Informationen �ber eine Assembly werden �ber folgende Attribute 
// gesteuert. �ndern Sie diese Attributswerte, um die Informationen zu modifizieren,
// die mit einer Assembly verkn�pft sind.
//
[assembly: AssemblyTitle("SimPe Family History Plugin")]
[assembly: AssemblyDescription("This is the Basic SimPe Plugin To Show Family History")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Ambertation")]
#if DEBUG
[assembly: AssemblyProduct("[DEBUG]")]
#else
	[assembly: AssemblyProduct("")]
#endif	
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]	

//
// Versionsinformationen f�r eine Assembly bestehen aus folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie k�nnen alle Werte oder die standardm��ige Revision und Buildnummer 
// mit '*' angeben:

[assembly: AssemblyVersion("1.69.0.0")]

//
// Um die Assembly zu signieren, m�ssen Sie einen Schl�ssel angeben. Weitere Informationen 
// �ber die Assemblysignierung finden Sie in der Microsoft .NET Framework-Dokumentation.
//
// Mit den folgenden Attributen k�nnen Sie festlegen, welcher Schl�ssel f�r die Signierung verwendet wird. 
//
// Hinweise: 
//   (*) Wenn kein Schl�ssel angegeben ist, wird die Assembly nicht signiert.
//   (*) KeyName verweist auf einen Schl�ssel, der im CSP (Crypto Service
//       Provider) auf Ihrem Computer installiert wurde. KeyFile verweist auf eine Datei, die einen
//       Schl�ssel enth�lt.
//   (*) Wenn die Werte f�r KeyFile und KeyName angegeben werden, 
//       werden folgende Vorg�nge ausgef�hrt:
//       (1) Wenn KeyName im CSP gefunden wird, wird dieser Schl�ssel verwendet.
//       (2) Wenn KeyName nicht vorhanden ist und KeyFile vorhanden ist, 
//           wird der Schl�ssel in KeyFile im CSP installiert und verwendet.
//   (*) Um eine KeyFile zu erstellen, k�nnen Sie das Programm sn.exe (Strong Name) verwenden.
//       Wenn KeyFile angegeben wird, muss der Pfad von KeyFile
//       relativ zum Projektausgabeverzeichnis sein:
//       %Project Directory%\obj\<configuration>. Wenn sich KeyFile z.B.
//       im Projektverzeichnis befindet, geben Sie das AssemblyKeyFile-Attribut 
//       wie folgt an: [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Das verz�gern der Signierung ist eine erweiterte Option. Weitere Informationen finden Sie in der
//       Microsoft .NET Framework-Dokumentation.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
