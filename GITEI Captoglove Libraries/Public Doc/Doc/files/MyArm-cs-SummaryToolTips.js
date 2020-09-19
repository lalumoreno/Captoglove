NDSummary.OnToolTipsLoaded("File:MyArm.cs",{12:"<div class=\"NDToolTip TClass LCSharp\"><div class=\"NDClassPrototype\" id=\"NDClassPrototype12\"><div class=\"CPEntry TClass Current\"><div class=\"CPModifiers\"><span class=\"SHKeyword\">public</span></div><div class=\"CPName\"><span class=\"Qualifier\">GITEICaptoglove.</span>&#8203;MyArm</div></div></div><div class=\"TTSummary\">Handles Captoglove module configured as forearm sensor.</div></div>",14:"<div class=\"NDToolTip TEnumeration LCSharp\"><div id=\"NDPrototype14\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public enum</span> eArmType</div><div class=\"TTSummary\">List of possible ways to use Captoglove module with this class:</div></div>",18:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype18\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public</span> MyArm(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">int</span>&nbsp;</td><td class=\"PName last\">nID,</td></tr><tr><td class=\"PType first\">eArmType&nbsp;</td><td class=\"PName last\">eType</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Initializes variables for Captoglove module configuration.</div></div>",19:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype19\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public</span> eArmType GetArmType()</div><div class=\"TTSummary\">Captoglove module use mode</div></div>",20:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype20\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public int</span> SetArmTransform(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">Transform&nbsp;</td><td class=\"PName last\">tArmObj,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">ePitchAxis,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">eYawAxis,</td></tr><tr><td class=\"PType first\">eModuleAxis&nbsp;</td><td class=\"PName last\">eRollAxis</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Attaches Captoglove module movement to arm transform.</div></div>",21:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype21\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetInitialArmRot(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRotX,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRotY,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fRotZ</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves initial rotation for arm transform.</div></div>",22:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype22\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetPitchLimits(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxUpRotation,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxDownRotation</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Creates the algorithm for pitch movement of the arm.</div></div>",23:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype23\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SetYawLimits(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxRightRotation,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">float</span>&nbsp;</td><td class=\"PName last\">fMaxLeftRotation</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Creates the algorithm for yaw movement of the arm.</div></div>",24:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype24\" class=\"NDPrototype NoParameterForm\"><span class=\"SHKeyword\">public void</span> MoveArm()</div><div class=\"TTSummary\">Updates arm transform rotation according with Captoglove module movement.</div></div>",25:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype25\" class=\"NDPrototype WideForm CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> SaveArmMovInFile(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">string</span>&nbsp;</td><td class=\"PName last\">sFileName</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"TTSummary\">Saves module data in file with following format: x arm rotation; y arm rotation; z arm rotation</div></div>"});