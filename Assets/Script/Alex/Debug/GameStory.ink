INCLUDE TestDialogs.ink
INCLUDE charlie-script.ink

EXTERNAL addFriend(friend_name)
EXTERNAL changePersonalityTrait(name, delta)

EXTERNAL changeHappiness(name, delta)

VAR player_name = "Player"
VAR player_happiness = 0
VAR player_friends_count = 0

VAR npc_name = "Npc"
VAR npc_happiness = 0

VAR helpful = 0
VAR easygoing = 0
VAR ambitious = 0
VAR adventurous = 0
VAR beverly = 0


-> charlie_meet_convo


=== function addFriend(friend_name) ===
// defined function to avoid compile time errors in INK
~ return

=== function changeHappiness(name, delta) ===
// defined function to avoid compile time errors in INK
~ return