extends Node2D

var tstamp : float = -1 # this var is the timestamp WHEN the object was created.
var time : float = 0 # global time if timestamp set, otherwise local
var keytime : float = 0
@export var type = 0 # type of the "key" functionality
var localtiming : bool = false # localtiming toggle
var key = "A"# key
var myindex = -1
var hit_stamp : float = -1
var hit_prescale : Vector2
var hit_rank : String = ""

# type 0
var pre_ring_time : float = 0.2
var ring_time : float = 0.5
var hit_timeframe : float = 0.2
var hit_tstamp : float = 0
var missed = false

func _ready() -> void:
	$KeysqMain.texture = ImageTexture.create_from_image(Image.load_from_file(Skins.get_resource_path("keysq_main.png")))
	$Stroke10PxGray.texture = ImageTexture.create_from_image(Image.load_from_file(Skins.get_resource_path("stroke_10px_gray.png")))


	if get_parent().name == "keybrd":
		position=get_parent().get_node(key).position
	$KeyLabel.text = key
	if tstamp == -1: # did timestamp change on ready? nah.
		
		tstamp = 0 # not breaking the animations
		localtiming=true # using local then.
	if type == 0 and get_tree().current_scene.name == "Editor":
		hit(pre_ring_time+ring_time+tstamp,"TAIPU!!!")

# ANIMATION NOTES:
# First, key bg appears with a fade and scale, Label appears with it
# the game waits for appear_delay to finish, and only then the ring starts scaling down from (3,3),
# until it reaches (1,1)

# (Hit!):
# The ring bounces to current+(2,2) 



func _process(delta: float) -> void: # independant from FPS which is crucial for a rhythm game.
	# HOW DO I SYNC THIS TO A TIMELINE?!
	# solution: clamp( time(current time) - tstamp(timestamp, if set) / duration(anim duration(seconds)), 0, 1)
	if localtiming: # local timing?
		time+=delta # yeah, im using local time
		
	else: # no locals? (megamind)
		time=get_tree().get_current_scene().viewer_node.time # yippee, using global then.
	keytime = time-tstamp
		
	
	
	if type == 0:
		if keytime>pre_ring_time+ring_time+hit_timeframe+1 and !missed:
			$Stroke10PxGray.visible = false
			$KeysqMain.visible = false
			$KeyLabel.visible = false
		if keytime < 0:
			if $KeysqMain.visible:
				$KeysqMain.visible = false
				$KeyLabel.visible = false
			if $Stroke10PxGray.visible:
				$Stroke10PxGray.visible = false
			
		if keytime > 0 and ($KeysqMain.scale.x < 1.0 or keytime < ring_time+pre_ring_time+hit_timeframe+1):
			$KeyLabel.text = key
			#mainbg anims
			if $KeysqMain.visible != true or $KeyLabel.visible != true:
				$KeysqMain.visible = true # making mainbg visible
				$KeyLabel.visible = true # making keylabel visible
			var mainbgscale_atemp = ease( clamp( keytime / (ring_time/2), 0, 1), 0.3) # calculating scale
			$KeysqMain.scale=Vector2(1,1) * Vector2(mainbgscale_atemp,mainbgscale_atemp) # applying scale
			
			var mainbgfade_atemp = clamp(keytime / (ring_time/6), 0 , 1) # calculating fade
			$KeysqMain.modulate.a = 1 * mainbgfade_atemp # applying fade
			
			#label anims
			
			
			var labelscale_atemp = ease( clamp( keytime / (ring_time/2), 0, 1), 0.3)
			$KeyLabel.scale=Vector2(2,2) - ( Vector2(1,1)*Vector2(labelscale_atemp,labelscale_atemp) ) # applying scale
			
			var labelfade_atemp = clamp(keytime / (ring_time/8), 0 , 1) # calculating fade
			$KeyLabel.modulate.a = 1 * labelfade_atemp # applying fade
			
		# ring anims
		if keytime < pre_ring_time:
			if $Stroke10PxGray.visible != false:
				$Stroke10PxGray.visible = false
		else:
			if $Stroke10PxGray.visible != true:
				$Stroke10PxGray.visible = true
				$Stroke10PxGray.z_index = 10
		if hit_stamp == -1 or time < hit_stamp:
			if keytime >= pre_ring_time and ($Stroke10PxGray.scale.x > 1.0 or keytime <= ring_time+pre_ring_time+hit_timeframe+1):
				if $Stroke10PxGray.visible != true:
					$Stroke10PxGray.visible = true
				var stroke10pxscale_atemp = clamp((keytime-pre_ring_time) / (ring_time), 0 , 1)
				$Stroke10PxGray.scale = Vector2(3,3) - (Vector2(1.999,1.999)*Vector2( stroke10pxscale_atemp, stroke10pxscale_atemp)) # applying scale
				
				var stroke10pxfade_atemp = clamp((keytime-pre_ring_time) / (ring_time/4), 0 , 1)
				$Stroke10PxGray.modulate.a = stroke10pxfade_atemp # applying scale
			
			if keytime>pre_ring_time+ring_time:
				if $KeysqMain.modulate.g != 1 or $KeyLabel.text != key:
					$KeyLabel.visible = true
					$Stroke10PxGray.visible = true
					$KeysqMain.modulate.g = 1
					$KeysqMain.modulate.b = 1
					$KeyLabel.add_theme_color_override("font_color", Color.BLACK)
					$KeyLabel.add_theme_constant_override("outline_size", 10)
					$KeyLabel.text = key
				pass
			if keytime >= pre_ring_time+ring_time+hit_timeframe:
				if !missed:
					miss()
				var mainbgfade_atemp = clamp((keytime-hit_timeframe-pre_ring_time-ring_time-0.2) / (ring_time/8), 0 , 1)
				$KeysqMain.modulate.a = 1 * 1-mainbgfade_atemp # applying fade
				$KeyLabel.modulate.a = 1 * 1-mainbgfade_atemp # applying fade
				if $KeyLabel.modulate.a == 0 && !get_tree().current_scene.name == "Editor":
					queue_free()
		if hit_rank!="" && time>hit_stamp and (keytime<pre_ring_time+ring_time+hit_timeframe):
			if time < hit_stamp+2:
				if !$Stroke10PxGray.visible or $KeyLabel.text != hit_rank: 
					$Stroke10PxGray.visible = true
					$KeysqMain.modulate.g = 1
					$KeysqMain.modulate.b = 1
					$KeyLabel.add_theme_color_override("font_color", Color.BLACK)
					$KeyLabel.add_theme_constant_override("outline_size", 10)
					$KeyLabel.text = hit_rank
					z_index = 10
				#var stroke10pxscale_atemp = clamp((time-hit_stamp) / (ring_time/3.3), 0 , 1)
				#print(stroke10pxscale_atemp)
				#$Stroke10PxGray.scale = hit_prescale + (Vector2(2,2)*Vector2( stroke10pxscale_atemp, stroke10pxscale_atemp)) # applying scale
				
				var stroke10pxfade_atemp = clamp((time-hit_stamp) / (ring_time/4), 0 , 1)
				$Stroke10PxGray.modulate.a = 1-stroke10pxfade_atemp # applying scale
				
				var mainbgscale_atemp = clamp((time-hit_stamp) / (ring_time/2), 0, 1) # calculating scale
				$KeysqMain.scale=Vector2(1,1) + Vector2(1,1) * Vector2(mainbgscale_atemp,mainbgscale_atemp) # applying scale
				
				var mainbgfade_atemp = clamp((time-hit_stamp) / (ring_time/6), 0 , 1) # calculating fade
				$KeysqMain.modulate.a = 1 * 1-mainbgfade_atemp # applying fade
				
				var labelscale_atemp = clamp( (time-hit_stamp) / (ring_time), 0, 1)
				$KeyLabel.scale=Vector2(1,1) + ( Vector2(1,1)*Vector2(labelscale_atemp,labelscale_atemp) ) # applying scale
				
				var labelfade_atemp = clamp((time-hit_stamp) / (ring_time/2), 0 , 1) # calculating fade
				$KeyLabel.modulate.a = 1 * 1-labelfade_atemp # applying fade
				if $KeyLabel.modulate.a == 0 && !get_tree().current_scene.name == "Editor":
					queue_free()
		if missed:
			z_index = 100
			$Stroke10PxGray.visible = false
			$KeyLabel.text = "MISS"
			$KeysqMain.modulate.g = 0.3
			$KeysqMain.modulate.b = 0.3
			$KeyLabel.add_theme_color_override("font_color", Color.WHITE)
			$KeyLabel.add_theme_constant_override("outline_size", 0)
			$KeyLabel.add_theme_font_size_override("font_size", 192)
		if keytime>pre_ring_time+ring_time+hit_timeframe  && !missed:
			$Stroke10PxGray.visible = false
			$KeysqMain.visible = false
			$KeyLabel.visible = false
		if keytime < 0:
			if $KeysqMain.visible:
				$KeysqMain.visible = false
				$KeyLabel.visible = false
			if $Stroke10PxGray.visible:
				$Stroke10PxGray.visible = false
func hit(hittime,hitrank):
	hit_rank = hitrank
	hit_stamp = hittime
	hit_prescale = $Stroke10PxGray.scale
func _unhandled_input(event):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_RIGHT:
		if $KeysqMain.get_rect().has_point(to_local(event.position)):
			if get_tree().current_scene.name == "Editor":
				if keytime > 0 && keytime<pre_ring_time+ring_time+hit_timeframe+0.15:
					get_tree().current_scene.delkey(myindex)
func miss():
	missed=true
	get_tree().current_scene.miss(myindex)
