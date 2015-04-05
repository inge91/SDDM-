import os
import csv
import re
import math
import numpy as np
import pylab as pl
import matplotlib.pyplot as plt
import matplotlib.patches as mpatches


def mean(values):
	return sum(values)/11.0
	
def variance(values):
	m = mean(values)
	total = 0
	for v in values:
		total += (v - m) * (v-m)
		
	return math.sqrt(total/11.0)

def main():
	estimateDict = {}	
	
	directoryContent = os.listdir(".")
	for file in directoryContent:
		if file[-4:] != ".csv":
			continue
		with open(file, 'rb') as csvfile:
			datareader = csv.reader(csvfile, delimiter = ";")
			i = 0;
			for row in datareader:
				if i == 0:
					i += 1
					continue
				coords = re.findall(r'\-*\d+\.\d+', row[0])
				coords = [float(x) for x in coords]	
				distance = int(row[1])
				if coords[0] > 0:
					degrees = 270
				elif coords[0] < 0:
					degrees = 90
				elif coords[2] >0:
					degrees = 0
				else:
					degrees = 180	
				currentEstimate = estimateDict.get((degrees, distance), [])
				estimateDict[(degrees, distance)] =  currentEstimate + [int(row[2])]
				i += 1
	i = 0
	meanEstimates1 = [mean(estimateDict[(0, 2)]), mean(estimateDict[(0, 4)]), mean(estimateDict[(0, 6)]), mean(estimateDict[(0, 8)])]
	varianceEstimate1 = [variance(estimateDict[(0, 2)]), variance(estimateDict[(0, 4)]),variance(estimateDict[(0, 6)]),variance(estimateDict[(0, 8)])]
	meanEstimates2 = [mean(estimateDict[(90, 2)]), mean(estimateDict[(90, 4)]), mean(estimateDict[(90, 6)]), mean(estimateDict[(90, 8)])]
	varianceEstimate2 = [variance(estimateDict[(90, 2)]), variance(estimateDict[(90, 4)]),variance(estimateDict[(90, 6)]),variance(estimateDict[(90, 8)])]
	meanEstimates3 = [mean(estimateDict[(180, 2)]), mean(estimateDict[(180, 4)]), mean(estimateDict[(180, 6)]), mean(estimateDict[(180, 8)])]
	varianceEstimate3 = [variance(estimateDict[(180, 2)]), variance(estimateDict[(180, 4)]),variance(estimateDict[(180, 6)]),variance(estimateDict[(180, 8)])]
	meanEstimates4 = [mean(estimateDict[(270, 2)]), mean(estimateDict[(270, 4)]), mean(estimateDict[(270, 6)]), mean(estimateDict[(270, 8)])]
	varianceEstimate4 = [variance(estimateDict[(270, 2)]), variance(estimateDict[(270, 4)]),variance(estimateDict[(270, 6)]),variance(estimateDict[(270, 8)])]
	
	# fig, axes = plt.subplots(nrows=2, ncols=2, figsize=(6,6))
	# axes[0, 0].set_title('0 degree')
	# axes[0, 1].set_title('90 degree')
	# axes[1, 0].set_title('180 degree')
	# axes[1, 1].set_title('270 degree')
	# axes[0,0].errorbar([2,4,6,8], meanEstimates1, yerr=varianceEstimate1, fmt="o")
	# axes[0,1].errorbar([2,4,6,8], meanEstimates2, yerr=varianceEstimate2, fmt="o")
	# axes[1,0].errorbar([2,4,6,8], meanEstimates3, yerr=varianceEstimate3, fmt="o")
	# axes[1,1].errorbar([2,4,6,8], meanEstimates4, yerr=varianceEstimate4, fmt="o")
	# axes[0, 0].axis((0, 10 ,-3, 3))
	# axes[0, 1].axis((0, 10 ,-3, 3))
	# axes[1, 0].axis((0, 10 ,-3, 3))
	# axes[1, 1].axis((0, 10 ,-3, 3))	
	# plt.show()
	
	N = 16	
	r = np.array(meanEstimates1 + meanEstimates2 + meanEstimates3 + meanEstimates4)

	theta = np.array(4 * [0] + 4 * [math.radians(90)] + 4*[math.radians(180)] + 4 * [math.radians(270)])

	area = np.array(varianceEstimate1 + varianceEstimate2 + varianceEstimate3 + varianceEstimate4) * 1000
	
	colors = ['red', 'blue', 'orange', 'lightcoral']
	ax = plt.subplot(111, polar=True)
	ax.set_rmax(9)
	ax.set_theta_offset(0.5 * math.pi)
	ax.set_theta_direction(-1)
	
	circle1 = pl.Circle((0.0, 0.0), 2, transform=ax.transData._b, color="red", fill=False, linewidth = 1.5)
	circle2 = pl.Circle((0.0, 0.0), 4, transform=ax.transData._b, color="blue", fill=False, linewidth = 1.5)
	circle3 = pl.Circle((0.0, 0.0), 6, transform=ax.transData._b, color="orange", fill=False, linewidth = 1.5)
	circle4 = pl.Circle((0.0, 0.0), 8, transform=ax.transData._b, color="lightcoral", fill=False, linewidth = 1.5)
	ax.add_artist(circle1)
	ax.add_artist(circle2)
	ax.add_artist(circle3)
	ax.add_artist(circle4)
	
	plt.title('Distance estimation of sound sources in distance experiment', y = -0.1)
	
	red = pl.Line2D([], [], color='red', marker='o',
                          markersize=8, label='Mean estimated position of sound spawned at 2 meter distance',  linestyle = 'None')
	blue = pl.Line2D([], [], color='blue', marker='o',
                          markersize=8, label='Mean estimated position of sound spawned at 4 meter distance',  linestyle = 'None')
	orange = pl.Line2D([], [], color='orange', marker='o',
                          markersize=8, label='Mean estimated position of sound spawned at 6 meter distance',  linestyle = 'None')
	coral = pl.Line2D([], [], color='lightcoral', marker='o',
                          markersize=8, label='Mean estimated position of sound spawned at 8 meter distance',  linestyle = 'None')
	plt.legend(handles=[red, blue, orange, coral], numpoints = 1, loc=(0.8,0.95), labelspacing = 0.2, handlelength = 0.5, fancybox = True)
	
	
	#errors = np.array(varianceEstimate1 + varianceEstimate2 + varianceEstimate3 + varianceEstimate4)

	#ax.errorbar(theta, r, yerr = errors, capsize=0, fmt = 'ro')
	#ax.plot(theta, r, "ro", c = colors, cmap=plt.cm.hsv)
	c2 = ax.scatter(theta, r, c=colors, s=16 * [75], cmap=plt.cm.hsv)
	c2.set_alpha(0.8)
	
	c = ax.scatter(theta, r, c=colors, s=area, cmap=plt.cm.hsv)
	c.set_alpha(0.25)

	plt.show()
	

if __name__ == "__main__":
	main()