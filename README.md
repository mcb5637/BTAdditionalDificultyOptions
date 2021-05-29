# BTAdditionalDificultyOptions

Allows difficulty settings to change company tags and call custom delegates.

Use custom settings:
Set `"ConstantType": "AdditionalSettings"` and fill `"ConstantName"` and `"ConstantValue"` with whatever you need.

Add Tags:
Set `"ConstantType": "AdditionalSettings"`, `"ConstantName": "AddSimGameTag"` and `"ConstantValue"` to the tag you want to add.

Remove Tags:
Set `"ConstantType": "AdditionalSettings"`, `"ConstantName": "RemoveSimGameTag"` and `"ConstantValue"` to the tag you want to remove (does nothing if the tag isnt there).

Add delegate to handle custom options:
- Add my dll as requirement in your dll
- Add my mod as dependency in your mod.json
- Write your delegate, it should be of type `Action<SimGameState, string, string, string>`, Parameters are `(SimGameState s, string ID, string ConstantName, string ConstantValue)`
- Make sure you do handle the case where s is null (new game screen)
- Add your delegate `BTAdditionalDificultyOptions.DifficultyOptionsMain.ApplySettings += YourFunc`
