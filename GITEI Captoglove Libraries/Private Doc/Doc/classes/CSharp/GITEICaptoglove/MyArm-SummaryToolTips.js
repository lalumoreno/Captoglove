NDSummary.OnToolTipsLoaded("CSharpClass:GITEICaptoglove.MyArm",{46:"<div class=\"NDToolTip TClass LCSharp\"><div class=\"NDClassPrototype\" id=\"NDClassPrototype46\"><div class=\"CPEntry TClass Current\"><div class=\"CPModifiers\"><span class=\"SHKeyword\">public</span></div><div class=\"CPName\"><span class=\"Qualifier\">GITEICaptoglove.</span>&#8203;MyArm</div></div></div><div class=\"TTSummary\">Handles Captoglove module configured as forearm sensor.</div></div>",48:"<div class=\"NDToolTip TEnumeration LCSharp\"><div id=\"NDPrototype48\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public enum</span> eArmType</div><div class=\"TTSummary\">List of possible ways to use Captoglove module with this class:</div></div>",52:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype52\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eArmType _eArmType</div></div>",53:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype53\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eModuleAxis _ePitchAxis</div></div>",54:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype54\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eModuleAxis _eYawAxis</div></div>",55:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype55\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eModuleAxis _eRollAxis</div></div>",56:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype56\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fArmXAngle</div></div>",57:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype57\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fArmYAngle</div></div>",58:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype58\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fArmZAngle</div></div>",59:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype59\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fPitchVarA</div></div>",60:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype60\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fPitchVarB</div></div>",61:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype61\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fYawVarA</div></div>",62:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype62\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fYawVarB</div></div>",63:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype63\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> Transform _tArm</div></div>",64:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype64\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> StreamWriter swArmWriter</div></div>",65:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype65\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private bool</span> bArmFile</div></div>",67:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype67\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public</span> MyArm(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">int</span>&nbsp;</td><td class=\"PName last\">nID,</td></tr><tr><td class=\"PType first\">eArmType&nbsp;</td><td class=\"PName last\">eType</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Initializes variables for Captoglove module configuration.</div></div>",68:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype68\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SetArmType(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">eArmType&nbsp;</td><td class=\"PName last\">eType</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves Captoglove module use mode.</div></div>",69:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype69\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> eArmType GetArmType()</div><div class=\"TTSummary\">Captoglove module use mode</div></div>",70:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype70\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public int</span> SetArmTransform(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">Transform&nbsp;</td><td class=\"PName last\">tArmObj,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">ePitchAxis,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">eYawAxis,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">eRollAxis</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Attaches Captoglove module movement to arm transform.</div></div>",71:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype71\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetInitialArmRot(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRotX,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRotY,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRotZ</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves initial rotation for arm transform.</div></div>",72:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype72\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private void</span> SetDefaultRotLimits()</div><div class=\"TTSummary\">Set the limits for the rotation of the arm transform.</div></div>",73:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype73\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetPitchLimits(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxUpRotation,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxDownRotation</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Creates the algorithm for pitch movement of the arm.</div></div>",74:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype74\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetYawLimits(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxRightRotation,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxLeftRotation</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Creates the algorithm for yaw movement of the arm.</div></div>",75:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype75\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public void</span> MoveArm()</div><div class=\"TTSummary\">Updates arm transform rotation according with Captoglove module movement.</div></div>",76:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype76\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private void</span> SetArmNewAngle()</div><div class=\"TTSummary\">Calculates arm transform rotation according with Captoglove module movement.</div></div>",77:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype77\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> AsignAngle2Axes(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fPitchA,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fYawA,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRollA</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Set rotation angle to each axis of the arm transform.</div></div>",78:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype78\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SaveArmMovInFile(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">string</span>&nbsp;</td><td class=\"PName last\">sFileName</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves module data in file with following format: x arm rotation; y arm rotation; z arm rotation</div></div>"});