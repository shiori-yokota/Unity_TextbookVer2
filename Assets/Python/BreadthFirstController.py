
########## Example ##########

Sequence_list = ["S3", "S4", "S7"]

queue = OPENLIST

while queue:
	label = queue.pop(0)
	if label == GOAL:
		CLOSEDLIST.append(label)
		# 解が発見されて終了

	if label not in CLOSEDLIST:
		CLOSEDLIST.append(label)
		queue += Sequence_list

#############################
