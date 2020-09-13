NDSummary.OnToolTipsLoaded("File:Module.cs",{30:"<div class=\"NDToolTip TClass LCSharp\"><div class=\"NDClassPrototype\" id=\"NDClassPrototype30\"><div class=\"CPEntry TClass Current\"><div class=\"CPModifiers\"><span class=\"SHKeyword\">public</span></div><div class=\"CPName\">Module</div></div></div><div class=\"TTSummary\">Handles Captoglove module. Used by MyHand and MyArm</div></div>",32:"<div class=\"NDToolTip TEnumeration LCSharp\"><div id=\"NDPrototype32\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected enum</span> peModuleType</div><div class=\"TTSummary\">List of possible ways to use Captoglove module:</div></div>",37:"<div class=\"NDToolTip TEnumeration LCSharp\"><div id=\"NDPrototype37\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public enum</span> eModuleAxis</div><div class=\"TTSummary\">List of axes:</div></div>",42:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype42\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> peModuleType _eModuleType</div></div>",43:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype43\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private int</span> _nModuleID</div></div>",44:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype44\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private string</span> _sModuleName</div></div>",45:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype45\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private bool</span> _bModuleInitialized</div></div>",46:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype46\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private bool</span> _bModuleStarted</div></div>",47:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype47\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private bool</span> _bPropertiesRead</div></div>",48:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype48\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private bool</span> _bLogEnabled</div></div>",49:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype49\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> IPeripheralCentral _IModuleCentral</div></div>",50:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype50\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> IBoardPeripheral _IModuleBoard</div></div>",51:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype51\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected float</span>[] pfaFingerSensorMaxValue</div></div>",52:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype52\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected float</span>[] pfaFingerSensorMinValue</div></div>",53:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype53\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected</span> BoardStreamEventArgs psEventTaredQuart</div></div>",54:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype54\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected</span> BoardStreamEventArgs psEventSensorState</div></div>",55:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype55\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected</span> BoardStreamEventArgs psEventLinearAcceleration</div></div>",57:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype57\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">protected void</span> InitModule(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">int</span>&nbsp;</td><td class=\"PName last\">nID,</td></tr><tr><td class=\"PType first\">peModuleType&nbsp;</td><td class=\"PName last\">eType</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Initializes variables for Captoglove module configuration</div></div>",58:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype58\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SetModuleType(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">peModuleType&nbsp;</td><td class=\"PName last\">eType</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves Captoglove module use mode</div></div>",59:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype59\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected</span> peModuleType GetModuleType()</div><div class=\"TTSummary\">Captoglove module use mode</div></div>",60:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype60\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SetModuleID(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">int</span>&nbsp;</td><td class=\"PName last\">nID</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves Captoglove module ID</div></div>",61:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype61\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected int</span> GetModuleID()</div><div class=\"TTSummary\">Captoglove module ID</div></div>",62:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype62\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private void</span> SetModuleName()</div><div class=\"TTSummary\">Creates and saves Captoglove module name</div></div>",63:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype63\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected string</span> GetModuleName()</div><div class=\"TTSummary\">Captoglove module name</div></div>",64:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype64\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SetModuleInitialized(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">bool</span>&nbsp;</td><td class=\"PName last\">b</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves whether Captoglove module is initialized or not</div></div>",65:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype65\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected bool</span> GetModuleInitialized()</div></div>",66:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype66\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SetModuleStarted(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">bool</span>&nbsp;</td><td class=\"PName last\">b</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves whether Captoglove module is started or not</div></div>",67:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype67\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected bool</span> GetModuleStarted()</div></div>",68:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype68\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SetPropertiesRead(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">bool</span>&nbsp;</td><td class=\"PName last\">b</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves whether Captoglove module properties have been read or not</div></div>",69:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype69\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">protected bool</span> GetPropertiesRead()</div></div>",70:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype70\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public void</span> EnableLog()</div><div class=\"TTSummary\">Enables log printing during your app execution</div></div>",71:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype71\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public void</span> DisableLog()</div><div class=\"TTSummary\">Disables log printing during your app execution</div></div>",72:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype72\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private bool</span> GetLogEnabled()</div></div>",73:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype73\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public int</span> Start()</div><div class=\"TTSummary\">Starts looking for Captoglove module peripheral</div></div>",74:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype74\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private async void</span> Central_PeripheralsChanged(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">object</span>&nbsp;</td><td class=\"PName last\">sender,</td></tr><tr><td class=\"PType first\">PeripheralsEventArgs&nbsp;</td><td class=\"PName last\">e</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Looks for Captoglove module ID among the modules that are connected to bluetooth</div></div>",75:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype75\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> Peripheral_PropertyChanged(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">object</span>&nbsp;</td><td class=\"PName last\">sender,</td></tr><tr><td class=\"PType first\">PropertyChangedEventArgs&nbsp;</td><td class=\"PName last\">e</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Detects if Captoglove module is connected to your app</div></div>",76:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype76\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> Peripheral_StreamReceived(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">object</span>&nbsp;</td><td class=\"PName last\">sender,</td></tr><tr><td class=\"PType first\">BoardStreamEventArgs&nbsp;</td><td class=\"PName last\">e</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Captures continuously stream events from Captoglove module connected</div></div>",77:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype77\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private void</span> SetProperties()</div><div class=\"TTSummary\">Set Captoglove module properties by default: Calibrate module, Tare module, Set time slots and Commit changes.</div></div>",78:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype78\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private async void</span> ReadProperties()</div><div class=\"TTSummary\">Read Captoglove module properties: Firmware version, Emulation mode, Time slots and Sensors calibration.</div></div>",79:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype79\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public async void</span> Stop()</div><div class=\"TTSummary\">Stops communication with Captoglove module</div></div>",80:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype80\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">protected void</span> TraceLog(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">string</span>&nbsp;</td><td class=\"PName last\">s</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Prints log lines during your app execution</div></div>"});