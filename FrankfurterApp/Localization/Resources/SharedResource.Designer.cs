﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrankfurterApp.Localization.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SharedResource {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SharedResource() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("FrankfurterApp.Localization.Resources.SharedResource", typeof(SharedResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string UNAUTHORIZED_EXCEPTION {
            get {
                return ResourceManager.GetString("UNAUTHORIZED_EXCEPTION", resourceCulture);
            }
        }
        
        internal static string FORBIDDEN_EXCEPTION {
            get {
                return ResourceManager.GetString("FORBIDDEN_EXCEPTION", resourceCulture);
            }
        }
        
        internal static string INTERNAL_SERVER_ERROR_EXCEPTION {
            get {
                return ResourceManager.GetString("INTERNAL_SERVER_ERROR_EXCEPTION", resourceCulture);
            }
        }
        
        internal static string CURRENCY_RATE_SERVICE_NOT_FOUND {
            get {
                return ResourceManager.GetString("CURRENCY_RATE_SERVICE_NOT_FOUND", resourceCulture);
            }
        }
        
        internal static string CURRENCY_NOT_FOUND {
            get {
                return ResourceManager.GetString("CURRENCY_NOT_FOUND", resourceCulture);
            }
        }
    }
}
