import sys
sys.path.append(PYTHON_LIB_PATH)
import random
import os.path
if os.path.exists('Python/Sources/qvalues.txt'):
	filename = 'Python/Sources/qvalues.txt'
else:
	filename = ''
