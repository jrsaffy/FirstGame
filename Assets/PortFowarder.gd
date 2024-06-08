extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	var upnp = UPNP.new()
	var discover_result = upnp.discover()
	
	if discover_result == UPNP.UPNP_RESULT_SUCCESS:
		if upnp.get_gateway() and upnp.get_gateway().is_valid_gateway():
			var udp_map_result = upnp.add_port_mapping(15973, 0, "Godot_udp", "UDP", 0)
			var tcp_map_result = upnp.add_port_mapping(15973,0, "Godot_tcp", "TCP", 0)
		
			if not udp_map_result == UPNP.UPNP_RESULT_SUCCESS:
				upnp.add_port_mapping(15973, 0, "", "UDP", 0)
			if not tcp_map_result == UPNP.UPNP_RESULT_SUCCESS:
				upnp.add_port_mapping(15973,0, "Godot_tcp", "TCP", 0)

	var externalip = upnp.query_external_address()
	print(externalip)
#	upnp.delete_port_mapping(15973, "UDP")
#	upnp.delete_port_mapping(15973, "TCP")
# Called every frame. 'delta' is the elapsed time since the previous frame.

