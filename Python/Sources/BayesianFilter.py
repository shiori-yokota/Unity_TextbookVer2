import sys
sys.path.append(PYTHON_LIB_PATH)
import random

# 迷路のサイズ
SIZE = 5
# 状態遷移確率
TRANS = 0.8
# 観測確率
KANSOKU = 0.7

# 初期化する
def init_SONZAI():
	Q = [0.0 for state in range(SIZE * SIZE)]
	return Q

def calcSONZAI(act, wall):
	opposite = -1
	#if act == 0: opposite = 2
	#elif act == 1: opposite = 3
	#elif act == 2: opposite = 0
	#elif act == 3: opposite = 1

	#for state in range(SIZE * SIZE):
	#	char_list = list(tmpWALLS[state])

	#	prePosSONZAI = -1.0;
	#	if act == 0:
	#		if state < 20: prePosSONZAI = preSONZAI[state + SIZE]
	#	elif act == 1: 
	#		if state != (0 and 5 and 10 and 15 and 20): prePosSONZAI = preSONZAI[state - 1]
	#	elif act == 2:
	#		if state > 0: prePosSONZAI = preSONZAI[state - SIZE]
	#	elif act == 3:
	#		if state != (4 and 9 and 14 and 19 and 24): prePosSONZAI = preSONZAI[state + 1]

	#	if int(char_list[opposite]) == 1:
	#		SONZAI[state] = preSONZAI[state] * (1 - TRANS)
	#	elif int(char_list[opposite]) == 0:
	#		if int(char_list[act]) == 1:
	#			SONZAI[state] = prePosSONZAI * TRANS + preSONZAI[state]
	#		elif int(char_list[act]) == 0:
	#			SONZAI[state] = prePosSONZAI * TRANS + preSONZAI[state] * (1 - TRANS)
	
def calcSENSOR(wall):
	for state in range(SIZE * SIZE):
		print(state)
		# それぞれの地点のセンサ情報と観測したセンサ情報が一致する場合
		if tmpWALLS[state] == wall:	# 各状態の壁情報 0(north)0(east)0(south)0(west)
			SENSOR[state] = KANSOKU;
		# それぞれの地点のセンサ情報と観測したセンサ情報が一致しない場合
		else:
			SENSOR[state] = (1 - KANSOKU) / 15
		

def information_integration():
	for state in range(SIZE * SIZE):
		G[state] = SONZAI[state] * SENSOR[state]

def normalization(g):
	total = 0.0
	for state in range(SIZE * SIZE):
		total = total + g[state]
	for state in range(SIZE * SIZE):
		SONZAI[state] = g[state] / total


################ main ###############

if ROBOTSTATE == 'INITIAL':
	# F_0(s_0)を初期化する
	SONZAI = init_SONZAI()

	#無情報
	preSONZAI = [(1.0 / (SIZE * SIZE)) for state in range(SIZE * SIZE)]
	
else:
	#1) ロボットが進んだので存在確率を計算する
	calcSONZAI(ACTION, WALL);	# ACTIONは送られてくる

	##2) センサ情報を計算する
	#SENSOR = init_SONZAI()
	#calcSENSOR(WALL) # robotのいる位置での壁情報 0(north)0(east)0(south)0(west)

	##3) 移動と観測情報の統合
	#G = init_SONZAI()
	#information_integration()

	##4) 正規化
	#normalization(G)
	
	#preSONZAI = SONZAI