﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileCabinetApp {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class res {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal res() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FileCabinetApp.res", typeof(res).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Available commands:.
        /// </summary>
        internal static string availableMessage {
            get {
                return ResourceManager.GetString("availableMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на There is an error in csv file.
        /// </summary>
        internal static string badCsvFile {
            get {
                return ResourceManager.GetString("badCsvFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling Create() with FirstName = &apos;{1}&apos;, LastName = &apos;{2}&apos;, DateOfBirth = &apos;{3}&apos;, Sex = &apos;{4}&apos;, Weight = &apos;{5}&apos;, Height = &apos;{6}&apos;.
        /// </summary>
        internal static string createLog {
            get {
                return ResourceManager.GetString("createLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Create() returned &apos;{1}&apos;.
        /// </summary>
        internal static string createResultLog {
            get {
                return ResourceManager.GetString("createResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Create method execution duration is {0}..
        /// </summary>
        internal static string createTime {
            get {
                return ResourceManager.GetString("createTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Using custom validation rules..
        /// </summary>
        internal static string customRule {
            get {
                return ResourceManager.GetString("customRule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Not valid date of birth.
        /// </summary>
        internal static string dateOfBirthException {
            get {
                return ResourceManager.GetString("dateOfBirthException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Date of birth: .
        /// </summary>
        internal static string dateOfBirthInputMessage {
            get {
                return ResourceManager.GetString("dateOfBirthInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Using default validation rules..
        /// </summary>
        internal static string defaultRule {
            get {
                return ResourceManager.GetString("defaultRule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на File Cabinet Application, developed by {0}.
        /// </summary>
        internal static string developerNameMessage {
            get {
                return ResourceManager.GetString("developerNameMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling Edit() with Id = &apos;{1}&apos;, FirstName = &apos;{2}&apos;, LastName = &apos;{3}&apos;, DateOfBirth = &apos;{4}&apos;, Sex = &apos;{5}&apos;, Weight = &apos;{6}&apos;, Height = &apos;{7}&apos;.
        /// </summary>
        internal static string editLog {
            get {
                return ResourceManager.GetString("editLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Edit() completed.
        /// </summary>
        internal static string editResultLog {
            get {
                return ResourceManager.GetString("editResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Edit method execution duration is {0}..
        /// </summary>
        internal static string editTime {
            get {
                return ResourceManager.GetString("editTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Exiting an application....
        /// </summary>
        internal static string exitMessage {
            get {
                return ResourceManager.GetString("exitMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Invalid parameters.
        /// </summary>
        internal static string exportArgumentsException {
            get {
                return ResourceManager.GetString("exportArgumentsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на All records are exported to file {0}.
        /// </summary>
        internal static string exportFileComplete {
            get {
                return ResourceManager.GetString("exportFileComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Use export *type of result file* *file name*.
        /// </summary>
        internal static string exportFormat {
            get {
                return ResourceManager.GetString("exportFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Unknow argument {0}.
        /// </summary>
        internal static string exportUnknownArgument {
            get {
                return ResourceManager.GetString("exportUnknownArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на File is exist - rewrite {0}? [Y/n].
        /// </summary>
        internal static string fileExistMessage {
            get {
                return ResourceManager.GetString("fileExistMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Id,First Name,Last Name,Date of Birth,Sex,Weight,Height.
        /// </summary>
        internal static string fileHeader {
            get {
                return ResourceManager.GetString("fileHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Using file storage.
        /// </summary>
        internal static string fileStorage {
            get {
                return ResourceManager.GetString("fileStorage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling FindDate() with DateOfBirth = &apos;{1}&apos;.
        /// </summary>
        internal static string findDateLog {
            get {
                return ResourceManager.GetString("findDateLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - FindDate() completed.
        /// </summary>
        internal static string findDateResultLog {
            get {
                return ResourceManager.GetString("findDateResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на FindDateOfBirth method execution duration is {0}..
        /// </summary>
        internal static string findDateTime {
            get {
                return ResourceManager.GetString("findDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling FindFirstName() with FirstName = &apos;{1}&apos;.
        /// </summary>
        internal static string findFirstNameLog {
            get {
                return ResourceManager.GetString("findFirstNameLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - FindFirstName() completed.
        /// </summary>
        internal static string findFirstNameResultLog {
            get {
                return ResourceManager.GetString("findFirstNameResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на FindFirstName method execution duration is {0}..
        /// </summary>
        internal static string findFirstNameTime {
            get {
                return ResourceManager.GetString("findFirstNameTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling FindLastName() with LastName = &apos;{1}&apos;.
        /// </summary>
        internal static string findLastNameLog {
            get {
                return ResourceManager.GetString("findLastNameLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - FindLastName() completed.
        /// </summary>
        internal static string findLastNameResultLog {
            get {
                return ResourceManager.GetString("findLastNameResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на FindLastName method execution duration is {0}..
        /// </summary>
        internal static string findLastNameTime {
            get {
                return ResourceManager.GetString("findLastNameTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Not valid first name.
        /// </summary>
        internal static string firstNameException {
            get {
                return ResourceManager.GetString("firstNameException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на First name: .
        /// </summary>
        internal static string firstNameInputMessage {
            get {
                return ResourceManager.GetString("firstNameInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling GetRecords().
        /// </summary>
        internal static string getRecordsLog {
            get {
                return ResourceManager.GetString("getRecordsLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - GetRecords() completed.
        /// </summary>
        internal static string getRecordsResultLog {
            get {
                return ResourceManager.GetString("getRecordsResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на GetRecords method execution duration is {0}..
        /// </summary>
        internal static string getRecordsTime {
            get {
                return ResourceManager.GetString("getRecordsTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling GetStat().
        /// </summary>
        internal static string getStatLog {
            get {
                return ResourceManager.GetString("getStatLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - GetStat() completed.
        /// </summary>
        internal static string getStatResultLog {
            get {
                return ResourceManager.GetString("getStatResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на GetStat method execution duration is {0}..
        /// </summary>
        internal static string getStatTime {
            get {
                return ResourceManager.GetString("getStatTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Not valid height.
        /// </summary>
        internal static string heightException {
            get {
                return ResourceManager.GetString("heightException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Height: .
        /// </summary>
        internal static string heightInputMessage {
            get {
                return ResourceManager.GetString("heightInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Enter your command, or enter &apos;help&apos; to get help..
        /// </summary>
        internal static string hintMessage {
            get {
                return ResourceManager.GetString("hintMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Invalid parameters.
        /// </summary>
        internal static string importException {
            get {
                return ResourceManager.GetString("importException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Cannot import from this file.
        /// </summary>
        internal static string importFailed {
            get {
                return ResourceManager.GetString("importFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на #{0}: {1}.
        /// </summary>
        internal static string importFailValidation {
            get {
                return ResourceManager.GetString("importFailValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} records were imported from {1}.
        /// </summary>
        internal static string importFileComplete {
            get {
                return ResourceManager.GetString("importFileComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Use export *type of result file* *file name*.
        /// </summary>
        internal static string importFormat {
            get {
                return ResourceManager.GetString("importFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Unknow argument {0}.
        /// </summary>
        internal static string importUnknownArgument {
            get {
                return ResourceManager.GetString("importUnknownArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Invalid argument..
        /// </summary>
        internal static string invalidArgument {
            get {
                return ResourceManager.GetString("invalidArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Invalid input: {0}..
        /// </summary>
        internal static string invalidInputMessage {
            get {
                return ResourceManager.GetString("invalidInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Invalid configuration data.
        /// </summary>
        internal static string invalidJsonData {
            get {
                return ResourceManager.GetString("invalidJsonData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Not valid rule.
        /// </summary>
        internal static string invalidRule {
            get {
                return ResourceManager.GetString("invalidRule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Invalid storage.
        /// </summary>
        internal static string invalidStorage {
            get {
                return ResourceManager.GetString("invalidStorage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Not valid last name.
        /// </summary>
        internal static string lastNameException {
            get {
                return ResourceManager.GetString("lastNameException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Last name: .
        /// </summary>
        internal static string lastNameInputMessage {
            get {
                return ResourceManager.GetString("lastNameInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling MakeSnapshot().
        /// </summary>
        internal static string makeSnapshotLog {
            get {
                return ResourceManager.GetString("makeSnapshotLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - MakeSnapshot() completed.
        /// </summary>
        internal static string makeSnapshotResultLog {
            get {
                return ResourceManager.GetString("makeSnapshotResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на MakeSnapshot method execution duration is {0}..
        /// </summary>
        internal static string makeSnapshotTime {
            get {
                return ResourceManager.GetString("makeSnapshotTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Using memory storage.
        /// </summary>
        internal static string memoryStorage {
            get {
                return ResourceManager.GetString("memoryStorage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Configuration file not found.
        /// </summary>
        internal static string missingJsonFile {
            get {
                return ResourceManager.GetString("missingJsonFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на There is no explanation for &apos;{0}&apos; command..
        /// </summary>
        internal static string noExplanationMessage {
            get {
                return ResourceManager.GetString("noExplanationMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Storage is empty..
        /// </summary>
        internal static string noRecords {
            get {
                return ResourceManager.GetString("noRecords", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на No records with this parameters.
        /// </summary>
        internal static string noRecordsMessage {
            get {
                return ResourceManager.GetString("noRecordsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &gt; .
        /// </summary>
        internal static string pointer {
            get {
                return ResourceManager.GetString("pointer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling Purge().
        /// </summary>
        internal static string purgeLog {
            get {
                return ResourceManager.GetString("purgeLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Purge() completed.
        /// </summary>
        internal static string purgeResultLog {
            get {
                return ResourceManager.GetString("purgeResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Purge method execution duration is {0}..
        /// </summary>
        internal static string purgeTime {
            get {
                return ResourceManager.GetString("purgeTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Record #{0} is created..
        /// </summary>
        internal static string recordCreateMessage {
            get {
                return ResourceManager.GetString("recordCreateMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Record #{0} doesn&apos;t exists..
        /// </summary>
        internal static string recordNotExist {
            get {
                return ResourceManager.GetString("recordNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Record #{0} is updated..
        /// </summary>
        internal static string recordUpdateMessage {
            get {
                return ResourceManager.GetString("recordUpdateMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Remove() failed. Record with Id = &apos;{1}&apos; is missing.
        /// </summary>
        internal static string removeFailedResultLog {
            get {
                return ResourceManager.GetString("removeFailedResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling Remove() with Id = &apos;{1}&apos;.
        /// </summary>
        internal static string removeLog {
            get {
                return ResourceManager.GetString("removeLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Remove() completed.
        /// </summary>
        internal static string removeResultLog {
            get {
                return ResourceManager.GetString("removeResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Record #{0} is removed..
        /// </summary>
        internal static string removeSuccess {
            get {
                return ResourceManager.GetString("removeSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Remove method execution duration is {0}..
        /// </summary>
        internal static string removeTime {
            get {
                return ResourceManager.GetString("removeTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Calling Restore().
        /// </summary>
        internal static string restoreLog {
            get {
                return ResourceManager.GetString("restoreLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} - Restore() completed.
        /// </summary>
        internal static string restoreResultLog {
            get {
                return ResourceManager.GetString("restoreResultLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Restore method execution duration is {0}..
        /// </summary>
        internal static string restoreTime {
            get {
                return ResourceManager.GetString("restoreTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Not valid sex.
        /// </summary>
        internal static string sexException {
            get {
                return ResourceManager.GetString("sexException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Sex: .
        /// </summary>
        internal static string sexInputMessage {
            get {
                return ResourceManager.GetString("sexInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Not valid weight.
        /// </summary>
        internal static string weightException {
            get {
                return ResourceManager.GetString("weightException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Weight: .
        /// </summary>
        internal static string weightInputMessage {
            get {
                return ResourceManager.GetString("weightInputMessage", resourceCulture);
            }
        }
    }
}
