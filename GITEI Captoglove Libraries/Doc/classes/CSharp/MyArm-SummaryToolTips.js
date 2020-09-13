NDSummary.OnToolTipsLoaded("CSharpClass:MyArm",{1:"<div class=\"NDToolTip TClass LCSharp\"><div class=\"NDClassPrototype\" id=\"NDClassPrototype1\"><div class=\"CPEntry TClass Current\"><div class=\"CPModifiers\"><span class=\"SHKeyword\">public</span></div><div class=\"CPName\">MyArm</div></div></div><div class=\"TTSummary\">Handles Captoglove module configured as forearm sensor.</div></div>",3:"<div class=\"NDToolTip TEnumeration LCSharp\"><div id=\"NDPrototype3\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public enum</span> eArmType</div><div class=\"TTSummary\">List of possible ways to use Captoglove module with this class:</div></div>",7:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype7\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eArmType _eArmType</div></div>",8:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype8\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eModuleAxis _ePitchAxis</div></div>",9:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype9\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eModuleAxis _eYawAxis</div></div>",10:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype10\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> eModuleAxis _eRollAxis</div></div>",11:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype11\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fArmXAngle</div></div>",12:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype12\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fArmYAngle</div></div>",13:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype13\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fArmZAngle</div></div>",14:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype14\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fPitchVarA</div></div>",15:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype15\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fPitchVarB</div></div>",16:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype16\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fYawVarA</div></div>",17:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype17\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private float</span> _fYawVarB</div></div>",18:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype18\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private</span> Transform _tArm</div></div>",20:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype20\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public</span> MyArm(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">int</span>&nbsp;</td><td class=\"PName last\">nID,</td></tr><tr><td class=\"PType first\">eArmType&nbsp;</td><td class=\"PName last\">eType</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Initializes variables for Captoglove module configuration</div></div>",21:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype21\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SetArmType(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">eArmType&nbsp;</td><td class=\"PName last\">eType</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves Captoglove module use mode</div></div>",22:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype22\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> eArmType GetArmType()</div><div class=\"TTSummary\">Captoglove module use mode</div></div>",23:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype23\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public int</span> SetArmTransform(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">Transform&nbsp;</td><td class=\"PName last\">tArmObj,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">ePitchAxis,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">eYawAxis,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">eRollAxis</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Attaches Captoglove module movement to arm transform.</div></div>",24:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype24\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private void</span> SetDefaultRotLimits()</div><div class=\"TTSummary\">Set the limits for the rotation of the arm transform</div></div>",25:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype25\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetPitchLimits(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxUpRotation,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxDownRotation</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Creates the algorithm for pitch movement of the arm</div></div>",26:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype26\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetYawLimits(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxRightRotation,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxLeftRotation</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Creates the algorithm for yaw movement of the arm</div></div>",27:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype27\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public void</span> MoveArm()</div><div class=\"TTSummary\">Updates arm transform rotation according with Captoglove module movement.</div></div>",28:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype28\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">private void</span> SetArmNewAngle()</div><div class=\"TTSummary\">Calculates arm transform rotation according with Captoglove module movement.</div></div>",29:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype29\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> AsignAngle2Axes(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fPitchA,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fYawA,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRollA</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Set rotation angle to each axis of the arm transform.</div></div>"});