extends Button

var viewer_node

func _on_pressed() -> void:
	viewer_node = get_tree().current_scene.viewer_node
	viewer_node.killkeys()
	viewer_node.song_offset = viewer_node.lvlmeta["pre_ring_time"]+viewer_node.lvlmeta["ring_time"]
	viewer_node.i=0
