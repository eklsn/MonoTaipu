extends TabBar

@export var EditTab : Node2D
@export var MetaTab : Node2D
@export var RhythmTab : Node2D
@export var FinishTab : Node2D



func _on_tab_changed(tab: int) -> void:
	EditTab.visible = false
	MetaTab.visible = false
	RhythmTab.visible = false
	FinishTab.visible = true
	match(tab):
		0:
			MetaTab.visible = true
		1:
			EditTab.visible = true
		2:
			RhythmTab.visible = true
		3:
			FinishTab.visible = true
