﻿
OPENLIST = ["S"]
CLOSEDLIST = []

list = [] 
list.append(["S3", 8])
list.append(["S4", 5])
list.append(["S6", 3])
list.append(["G", 0])

stack = OPENLIST

#while stack:
#	label = stack.pop(0)
#	if label == GOAL:
#		CLOSEDLIST.append(label)
#		# 解が発見されて終了
#		UnityEngine.Debug.Log('Finish python code!!')

#	if label not in CLOSEDLIST:
#		CLOSEDLIST.append(label)
#		stack = list + OPENLIST

# CLOSEDLISTに状態名を入れてください
flat_list = []
for e in list:
	flat_list.extend(e)
list_str = flat_list[0::2]

CLOSEDLIST = OPENLIST + list_str