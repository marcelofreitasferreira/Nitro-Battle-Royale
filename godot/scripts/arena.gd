extends Node3D

var player: CharacterBody3D
var camera: Camera3D
var movement := Vector2.ZERO
var look_drag := false
var last_look := Vector2.ZERO
var pitch := -0.25
var yaw := 0.0
var jump_requested := false
var fire_requested := false
var health := 100
var info: Label

func _ready() -> void:
    var sun := DirectionalLight3D.new()
    sun.rotation_degrees = Vector3(-50, -30, 0)
    add_child(sun)
    var world := WorldEnvironment.new()
    var env := Environment.new()
    env.background_mode = Environment.BG_COLOR
    env.background_color = Color(0.47, 0.73, 0.95)
    env.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR
    env.ambient_light_color = Color(0.85, 0.85, 0.85)
    world.environment = env
    add_child(world)
    _box(Vector3(0, -0.5, 0), Vector3(64, 1, 64), Color(0.23, 0.57, 0.27))
    for i in range(16):
        var a := TAU * float(i) / 16.0
        _box(Vector3(cos(a) * 19.0, 1.3, sin(a) * 19.0), Vector3(3, 2.6, 3), Color(0.55, 0.48, 0.39))
    for i in range(7):
        _box(Vector3(-20 + i * 6, 1, 8), Vector3(3, 2, 3), Color(0.67, 0.65, 0.51))
    player = CharacterBody3D.new()
    player.name = "Player"
    player.position = Vector3(0, 2, 0)
    add_child(player)
    var shape := CapsuleShape3D.new()
    shape.radius = 0.45
    shape.height = 1.8
    var collider := CollisionShape3D.new()
    collider.shape = shape
    player.add_child(collider)
    var mesh := MeshInstance3D.new()
    var capsule := CapsuleMesh.new()
    capsule.radius = 0.45
    capsule.height = 1.8
    mesh.mesh = capsule
    mesh.material_override = _material(Color(0.10, 0.55, 0.93))
    player.add_child(mesh)
    camera = Camera3D.new()
    camera.current = true
    add_child(camera)
    _build_ui()

func _material(color: Color) -> StandardMaterial3D:
    var mat := StandardMaterial3D.new()
    mat.albedo_color = color
    return mat

func _box(pos: Vector3, size: Vector3, color: Color) -> void:
    var body := StaticBody3D.new()
    body.position = pos
    add_child(body)
    var mesh := MeshInstance3D.new()
    var box := BoxMesh.new()
    box.size = size
    mesh.mesh = box
    mesh.material_override = _material(color)
    body.add_child(mesh)
    var collision := CollisionShape3D.new()
    var shape := BoxShape3D.new()
    shape.size = size
    collision.shape = shape
    body.add_child(collision)

func _physics_process(delta: float) -> void:
    var keys := Vector2(
        float(Input.is_key_pressed(KEY_D)) - float(Input.is_key_pressed(KEY_A)),
        float(Input.is_key_pressed(KEY_S)) - float(Input.is_key_pressed(KEY_W))
    )
    var axis := (movement + keys).limit_length(1.0)
    var forward := Vector3(-sin(yaw), 0, -cos(yaw))
    var right := Vector3(cos(yaw), 0, -sin(yaw))
    var direction := (right * axis.x + forward * -axis.y).normalized()
    player.velocity.x = direction.x * 6.0
    player.velocity.z = direction.z * 6.0
    if not player.is_on_floor():
        player.velocity.y -= 20.0 * delta
    elif jump_requested or Input.is_key_pressed(KEY_SPACE):
        player.velocity.y = 7.0
    jump_requested = false
    player.move_and_slide()
    if direction.length() > 0.1:
        player.rotation.y = atan2(-direction.x, -direction.z)
    var target := player.global_position + Vector3(0, 1.4, 0)
    var desired := target + Vector3(sin(yaw) * cos(pitch), -sin(pitch), cos(yaw) * cos(pitch)) * 5.0
    camera.global_position = camera.global_position.lerp(desired, min(1.0, delta * 10.0))
    camera.look_at(target)
    if fire_requested:
        _fire()
        fire_requested = false

func _fire() -> void:
    var from := camera.global_position
    var to := from - camera.global_transform.basis.z * 70.0
    var query := PhysicsRayQueryParameters3D.create(from, to)
    query.exclude = [player.get_rid()]
    var hit := get_world_3d().direct_space_state.intersect_ray(query)
    info.text = "Tiro: " + ("acertou obstaculo" if not hit.is_empty() else "sem alvo")

func _build_ui() -> void:
    var canvas := CanvasLayer.new()
    add_child(canvas)
    var root := Control.new()
    root.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
    root.mouse_filter = Control.MOUSE_FILTER_PASS
    canvas.add_child(root)
    info = Label.new()
    info.text = "NITRO BATTLE ROYALE | PROTOTIPO v0.3"
    info.position = Vector2(20, 16)
    root.add_child(info)
    _button(root, "▲", Vector2(115, -195), true, "forward")
    _button(root, "◀", Vector2(35, -115), true, "left")
    _button(root, "▼", Vector2(115, -115), true, "back")
    _button(root, "▶", Vector2(195, -115), true, "right")
    _button(root, "PULAR", Vector2(-245, -120), false, "jump")
    _button(root, "ATIRAR", Vector2(-135, -215), false, "fire")

func _button(root: Control, title: String, pos: Vector2, left: bool, action: String) -> void:
    var btn := Button.new()
    btn.text = title
    btn.custom_minimum_size = Vector2(74, 68)
    btn.set_anchors_preset(Control.PRESET_BOTTOM_LEFT if left else Control.PRESET_BOTTOM_RIGHT)
    btn.position = pos
    btn.offset_left = pos.x
    btn.offset_top = pos.y
    btn.offset_right = pos.x + 74
    btn.offset_bottom = pos.y + 68
    root.add_child(btn)
    btn.button_down.connect(func(): _control(action, true))
    btn.button_up.connect(func(): _control(action, false))

func _control(action: String, pressed: bool) -> void:
    match action:
        "forward": movement.y = -1.0 if pressed else 0.0
        "back": movement.y = 1.0 if pressed else 0.0
        "left": movement.x = -1.0 if pressed else 0.0
        "right": movement.x = 1.0 if pressed else 0.0
        "jump": jump_requested = pressed
        "fire": fire_requested = pressed

func _unhandled_input(event: InputEvent) -> void:
    if event is InputEventScreenDrag:
        if event.position.x > get_viewport().get_visible_rect().size.x * 0.35:
            yaw -= event.relative.x * 0.004
            pitch = clamp(pitch + event.relative.y * 0.004, -0.8, 0.5)
    elif event is InputEventMouseMotion and Input.is_mouse_button_pressed(MOUSE_BUTTON_RIGHT):
        yaw -= event.relative.x * 0.004
        pitch = clamp(pitch + event.relative.y * 0.004, -0.8, 0.5)
