
########## Example ##########

OPENLIST = ["S"]
CLOSEDLIST = []

Sequence_list = ["S3", "S4", "S1", "S6", "G", "S2", "S7", "S8", "S5", "S10","S9"]

stack = OPENLIST

while stack:
	label = stack.pop(0)
	if label == "G":
		CLOSEDLIST.append(label)
		# 解が発見されて終了

	if label not in CLOSEDLIST:
		CLOSEDLIST.append(label)
		stack = Sequence_list

#############################
