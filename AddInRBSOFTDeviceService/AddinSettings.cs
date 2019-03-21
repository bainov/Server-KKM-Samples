using System.EnterpriseServices;

// https://rsdn.org/article/dotnet/cs1c.xml
// Конфигурирование библиотеки
[assembly: ApplicationAccessControl(AccessChecksLevel = AccessChecksLevelOption.ApplicationComponent)]
[assembly: ApplicationName("AddIn.RBSoftPrintService")]
[assembly: ApplicationActivation(ActivationOption.Library)]
//[assembly: AssemblyKeyFile("key.snk")]
// подписывание производится в свойствах проекта