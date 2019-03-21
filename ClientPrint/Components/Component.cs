using System;
using System.Collections;
using System.EnterpriseServices;
using System.Linq;
using System.Reflection;

// ReSharper disable All
#pragma warning disable 1591

namespace ClientPrint.Components
{
    #region

    public enum HRESULT : uint
    {
        S_OK = 0x00000000,
        S_FALSE = 0x00000001,
        E_FAIL = 0x80004005
    }

    public enum ERRORCODE : short
    {
        ADDIN_E_NONE = 1000,
        ADDIN_E_ORDINARY = 1001,
        ADDIN_E_ATTENTION = 1002,
        ADDIN_E_IMPORTANT = 1003,
        ADDIN_E_VERY_IMPORTANT = 1004,
        ADDIN_E_INFO = 1005,
        ADDIN_E_FAIL = 1006,
        ADDIN_E_MSGBOX_ATTENTION = 1007,
        ADDIN_E_MSGBOX_INFO = 1008,
        ADDIN_E_MSGBOX_FAIL = 1009
    }

    #endregion

    public abstract class Component : ServicedComponent, IInitDone, ILanguageExtender
    {
        private IErrorLog _errorLog = null;

        protected virtual void Init(object connection)
        {
            //TODO Удалить  переопределение в наследниках, так как по факту они не представляют Реализации
            //asyncEvent = (IAsyncEvent)connection;
            //statusLine = (IStatusLine)connection;
        }

        private Hashtable _name_methods;
        private Hashtable _name_properties;
        private MethodInfo[] _methods;
        private PropertyInfo[] _properties;

        protected void Error(string message)
        {
            try
            {
                var ei = new System.Runtime.InteropServices.ComTypes.EXCEPINFO();

                ei.wCode = (short)ERRORCODE.ADDIN_E_FAIL;
                ei.bstrSource = this.ToString();
                ei.bstrDescription = message;

                _errorLog.AddError(null, ref ei);
            }
            catch
            {
            }
        }

        #region Реализация IInitDone

        /// <summary>
        /// Инициализация компонента
        /// </summary>
        /// <param name="connection">reference to IDispatch</param>
        HRESULT IInitDone.Init(object connection)
        {
            try
            {
                // _asyncEvent = (IAsyncEvent)connection;
                // _statusLine = (IStatusLine)connection;
                _errorLog = (IErrorLog)connection;

                Init(connection);

                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                Error(ex.ToString());
                return HRESULT.E_FAIL;
            }
        }

        /// <summary>
        /// Возвращается информация о компоненте
        /// </summary>
        /// <param name="info">Component information</param>
        HRESULT IInitDone.GetInfo(ref object[] info)
        {
            info[0] = 2000;
            return HRESULT.S_OK;
        }

        /// <summary>
        /// ХЗ
        /// </summary>
        HRESULT IInitDone.Done()
        {
            return HRESULT.S_OK;
        }

        #endregion Реализация IInitDone

        #region Реализация ILanguageExtender

        /// <summary>
        /// Register component in 1C
        /// </summary>
        /// <param name="extensionName"></param>
        HRESULT ILanguageExtender.RegisterExtensionAs(out string extensionName)
        {
            try
            {
                _name_methods = new Hashtable();
                _name_properties = new Hashtable();

                Type type = this.GetType();

                _methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).Where(a => a.DeclaringType.IsSubclassOf(typeof(Component))).ToArray();

                _properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).Where(a => a.DeclaringType.IsSubclassOf(typeof(Component))).ToArray();

                for (int i = 0; i < _methods.Length; i++)
                    _name_methods.Add(_methods[i].Name, i);

                for (int i = 0; i < _properties.Length; i++)
                    _name_properties.Add(_properties[i].Name, i);

                extensionName = type.Name;
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                extensionName = null;
                return HRESULT.S_OK;
            }
        }

        #region Методы

        /// <summary>
        /// Возвращает количество методов
        /// </summary>
        /// <param name="Methods">Количество методов</param>
        /// <remarks>
        /// <prototype>
        /// [helpstring("method GetNMethods")]
        /// HRESULT GetNMethods([in,out]long *plMethods);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.GetNMethods(ref int Methods)
        {
            try
            {
                Methods = _methods.Length;
                return HRESULT.S_OK;
            }
            catch
            {
                Methods = 0;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Возвращает идентификатор метода
        /// </summary>
        /// <param name="methodName">Имя метода</param>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <remarks>
        /// <prototype>
        /// [helpstring("method FindMethod")]
        /// HRESULT FindMethod(BSTR bstrMethodName,[in,out]long *plMethodNum);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.FindMethod(string methodName, out int methodNum)
        {
            try
            {
                methodNum = (Int32)_name_methods[methodName];
                return HRESULT.S_OK;
            }
            catch
            {
                methodNum = -1;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Возвращает имя метода по его идентификатору
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="methodAlias"></param>
        /// <param name="methodName">Имя метода</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetMethodName([in]long lMethodNum,[in]long lMethodAlias,[in,out]BSTR *pbstrMethodName);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.GetMethodName(int methodNum, int methodAlias, ref string methodName)
        {
            try
            {
                methodName = _methods[methodNum].Name;
                return HRESULT.S_OK;
            }
            catch
            {
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Возвращает число параметров метода по его идентификатору
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="pParams">Число параметров</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetNParams([in]long lMethodNum,[in,out]long *plParams);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.GetNParams(int methodNum, ref int pParams)
        {
            try
            {
                pParams = _methods[methodNum].GetParameters().Length;
                return HRESULT.S_OK;
            }
            catch
            {
                pParams = -1;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Указывает, что у метода есть возвращаемое значение
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="retValue">Наличие возвращаемого значения</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT HasRetVal([in]long lMethodNum,[in,out]BOOL *pboolRetValue);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.HasRetVal(int methodNum, ref bool retValue)
        {
            try
            {
                retValue = _methods[methodNum].ReturnType != typeof(void);
                return HRESULT.S_OK;
            }
            catch
            {
                retValue = false;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Вызов метода как функции с использованием идентификатора
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="retValue">Возвращаемое значение</param>
        /// <param name="pParams">Параметры</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT CallAsFunc( [in]long lMethodNum,[in,out] VARIANT *pvarRetValue,
        ///       [in] SAFEARRAY (VARIANT)*paParams);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.CallAsFunc(int methodNum, ref object retValue, ref object[] pParams)
        {
            try
            {
                retValue = _methods[methodNum].Invoke(this, pParams);
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                Error(ex.Message.ToString());
                retValue = null;
                return HRESULT.E_FAIL;
            }
        }

        /// <summary>
        /// Вызов метода как процедуры с использованием идентификатора
        /// </summary>
        /// <param name="methodNum">Идентификатор метода</param>
        /// <param name="pParams">Параметры</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT CallAsProc([in]long lMethodNum,[in] SAFEARRAY (VARIANT) *paParams);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.CallAsProc(int methodNum, ref object[] pParams)
        {
            try
            {
                _methods[methodNum].Invoke(this, pParams);
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                Error(ex.InnerException.ToString());
                return HRESULT.E_FAIL;
            }
        }

        /// <summary>
        /// Возвращает значение параметра метода по умолчанию
        /// </summary>
        /// <param name="methodNum"></param>
        /// <param name="paramNum"></param>
        /// <param name="paramDefValue"></param>
        HRESULT ILanguageExtender.GetParamDefValue(int methodNum, int paramNum, ref object paramDefValue)
        {
            try
            {
                //
                paramDefValue = null;
                return HRESULT.S_FALSE;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                return HRESULT.S_FALSE;
            }
        }

        #endregion Методы

        #region Свойства

        /// <summary>
        /// Возвращается количество свойств
        /// </summary>
        /// <param name="props">Количество свойств </param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetNProps([in,out]long *plProps);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.GetNProps(ref int props)
        {
            try
            {
                props = _properties.Length;
                return HRESULT.S_OK;
            }
            catch
            {
                props = 0;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Возвращает целочисленный идентификатор свойства, соответствующий
        /// переданному имени
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <param name="propNum">Идентификатор свойства</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT FindProp([in]BSTR bstrPropName,[in,out]long *plPropNum);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.FindProp(string propName, ref long propNum)
        {
            try
            {
                propNum = (Int32)_name_properties[propName];
                return HRESULT.S_OK;
            }
            catch
            {
                propNum = -1;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Возвращает имя свойства, соответствующее
        /// переданному целочисленному идентификатору
        /// </summary>
        /// <param name="propNum">Идентификатор свойства</param>
        /// <param name="propAlias"></param>
        /// <param name="propName">Имя свойства</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT GetPropName([in]long lPropNum,[in]long lPropAlias,[in,out]BSTR *pbstrPropName);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.GetPropName(int propNum, int propAlias, out string propName)
        {
            try
            {
                propName = _properties[propNum].Name;
                return HRESULT.S_OK;
            }
            catch
            {
                propName = null;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Возвращает значение свойства.
        /// </summary>
        /// <param name="propNum">Идентификатор свойства </param>
        /// <param name="propVal">Значение свойства</param>
        /// <remarks>
        /// <prototype>
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.GetPropVal(int propNum, out object propVal)
        {
            try
            {
                propVal = _properties[propNum].GetValue(this, null);
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                Error(ex.InnerException.ToString());
                propVal = null;
                return HRESULT.E_FAIL;
            }
        }

        /// <summary>
        /// Устанавливает значение свойства.
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <param name="propVal">Значение свойства</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT SetPropVal([in]long lPropNum,[in]VARIANT *varPropVal);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.SetPropVal(int propName, ref object propVal)
        {
            try
            {
                _properties[propName].SetValue(this, propVal, null);
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                Error(ex.InnerException.ToString());
                return HRESULT.E_FAIL;
            }
        }

        /// <summary>
        /// Определяет, можно ли читать значение свойства
        /// </summary>
        /// <param name="propNum"> Идентификатор свойства </param>
        /// <param name="propRead">Флаг читаемости</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT IsPropReadable([in]long lPropNum,[in,out]BOOL *pboolPropRead);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.IsPropReadable(int propNum, out bool propRead)
        {
            try
            {
                propRead = _properties[propNum].CanRead;
                return HRESULT.S_OK;
            }
            catch
            {
                propRead = false;
                return HRESULT.S_FALSE;
            }
        }

        /// <summary>
        /// Определяет, можно ли изменять значение свойства
        /// </summary>
        /// <param name="propNum">Идентификатор свойства</param>
        /// <param name="propWrite">Флаг записи</param>
        /// <remarks>
        /// <prototype>
        /// HRESULT IsPropWritable([in]long lPropNum,[in,out]BOOL *pboolPropWrite);
        /// </prototype>
        /// </remarks>
        HRESULT ILanguageExtender.IsPropWritable(int propNum, out bool propWrite)
        {
            try
            {
                propWrite = _properties[propNum].CanWrite;
                return HRESULT.S_OK;
            }
            catch
            {
                propWrite = false;
                return HRESULT.S_FALSE;
            }
        }

        #endregion Свойства

        #endregion Реализация ILanguageExtender
    }
}