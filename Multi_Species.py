""" The typical Lotka-Volterra Model simulated using scipy """

__author__ = 'Orestes (o.gutierrez-al-khudhairy@imperial.ac.uk)'
__version__ = '0.0.1'
import numpy
import sys #allows us to use input arguments
import scipy as sc 
import scipy.integrate as integrate
import math
import pylab as p #Contains matplotlib for plotting

#import matplotlip.pylab as p #Some people might need to do this

counter=0 #counter

#create initial plant species)
X= sc.array([10])
a = sc.array([-0.05])
t = sc.linspace(0, 10,  1000) 
m = sc.array([1])
counter=0


def dX_dt(X, t, param1, param2):
	growth=param1
	com_M=param2
	spec=len(X)
	Xs = [a[i]*X[i] for i in range(spec)]	
	return (A.dot(X) + X*growth)	
	

	
	
	
def store(old, new):
	old=old.reshape((1,len(old)))
	lold=old.shape[1]
	lnew=new.shape[1]
	if (lold==lnew):
		X=sc.vstack((old,new))
	elif (lold>lnew):
		new =sc.hstack(([0]*(lold-lnew),new))
		X=X=sc.vstack((old,new))
	elif (lnew>lold):
		old =sc.hstack((old,[0]*(lnew-lold)))
		X=X=sc.vstack((old,new))
	return(X)
	

spec = len(m)
X_mat = sc.zeroes(T_len,Max_N) #pre-allocate array
Spec = 1

while (spec < Max_N):
	Spec = len(m)
	
	#time step counter
	counter=counter+1
	print(spec)
	print(counter)
	
	#integration process
	X_new, infodict = integrate.odeint(dX_dt, X, t, args= (m,a) , full_output=True)
	
	globals()['Populations%s' % counter]= X_new
	#X_s_new =X_new.T
	#if (spec == 1):
	#	if (counter ==1):
	#		X_store=X_s_new
	#	else:
	#		X=X.reshape(1,1)
	#		X_store=sc.hstack((X,X_s_new))
	#else:
	#	X_store= store(X,X_s_new)
	
	#create species that want to invade
	prob = numpy.random.rand(1)
	
	### CREATION OF INVADER ###
	
	
	if (prob <0.75):
		print("New species incoming")
	# Assign initial population density of invader
		X=sc.hstack((X_new[len(X)-1][:],math.floor(numpy.random.rand(1)*10))) #only store the last values of old populations using X[len(X)-1][:]
		
		m = sc.hstack((m, numpy.random.rand(1))) #create reproduction rates
		if(numpy.random.rand(1) < 0.3): 		#make paramter negative for certain prob
			m[len(m)-1] = -1*m[len(m)-1]
			
		
		#inter/intra-specific parameterisation
		new_a_row = sc.zeros(len(X)) 
		new_a_col = sc.zeros(len(X)-1)
		
		#make parameters negative with certain probability 
		for k in range(len(X)):
			new_a_row[k] = numpy.random.rand(1)
			if(numpy.random.rand(1) < 0.3): 
				new_a_row[k] = -1*new_a_row[k]
		for k in range(len(X)-1):	
			new_a_col[k] = numpy.random.rand(1)
			if(numpy.random.rand(1) < 0.3): 
				new_a_col[k] = -1*new_a_col[k]
		#store new parameters
		if (len(a)==1):
			a = sc.hstack((a,new_a_col))
		else:
			a = sc.hstack((a,new_a_col.reshape(spec,1)))
		a = sc.vstack((a,new_a_row))
		
	else:
		X=X_new[len(X)-1][:]#only store the last population numbers, don't need the historical time-series anymore.
		
	spec = len(m)				
	
	#prepare calculation of survival of species
	if (spec > 1):
		A_surv = sc.zeros((spec,spec))
		for i in range(spec):
			for j in range(spec):
				A_surv[i][j] =a[i][j]*X[j]				
	
	#delete species that go extinct
		for x in range(spec):
			if (sum(A_surv[x]-m[x])<=0):
				print("Extinction incoming")
				sc.delete(a, (x), axis=0)
				sc.delete(a, (x), axis=1)
				sc.delete(X, (x), axis=0)
				sc.delete(m, (x), axis=0)
	
x, y, z= X_new.T# What's this for? Retrieves necessairy population time series data needed for potting

f1 = p.figure() #Open empty figure object
p.plot(t, x, 'g-', label='x density') # Plot
p.plot(t, y, 'b-', label='y density')
p.plot(t, z, 'r-', label='z density')
p.grid()
p.legend(loc='best')
p.xlabel('Time')
#p.text(-30,3.5,"a=",fontsize='small', color='green') 
#p.text(-20,3.5,a,fontsize='small', color='red') 
#p.text(-30,2.5,"b_x=",fontsize='small', color='green') 
#p.text(-20,2.5,b_x ,fontsize='small', color='red')
#p.text(-30,1.5,"d_x=",fontsize='small', color='green') 
#p.text(-20,1.5,d_x ,fontsize='small', color='red') 
#p.text(-30,0.5,"b_y=",fontsize='small', color='green') 
#p.text(-20,0.5,b_y ,fontsize='small', color='red')
#p.text(-30,-1.5,"d_y=",fontsize='small', color='green') 
#p.text(-20,-1.5,d_y ,fontsize='small', color='red')


p.ylabel('Population')
p.title('x-y population dynamics')
#p.show()

f1.savefig('MultiSpecies_Dynamics.pdf') #Save figure

