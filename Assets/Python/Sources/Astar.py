OPENLIST = ["S"]
CLOSEDLIST = []

list = [] 
list.append(["S3", 8])
list.append(["S4", 5])
list.append(["S6", 3])
list.append(["G", 0])

# CLOSEDLISTに状態名を入れてください
flat_list = []
for e in list:
	flat_list.extend(e)
list_str = flat_list[0::2]

CLOSEDLIST = OPENLIST + list_str
