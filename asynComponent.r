if(!is.element("apcluster", installed.packages()[,1]))
{
	local({r <- getOption("repos")
		r["CRAN"] <- "http://cran.csie.ntu.edu.tw/"
		options(repos=r)})
	install.packages("apcluster")

}else{ print("apcluster already installed.")}

require("apcluster")
#print(commandArgs(TRUE)[1])

path <- "C:\\Users\\USER\\Documents\\GitHub\\SRC_tool\\"

input <- paste(path, "sample.txt", sep="")

# with power 
measure1 <- read.table(input,sep="",header=T)
similar1 <- expSimMat(measure1, r=2)
cluster1 <- apcluster(similar1, q=0.2)
 
# without power
measure2 <- measure1[,1:length(measure1)-1]
similar2 <- expSimMat(measure2, r=2)
cluster2 <- apcluster(similar2, q=0.9)

list2 <- c(1:length(cluster2@clusters))
list1 <- c(1:length(cluster1@clusters))


# For each cluster in cluster2
for(i in list2)
{
	cat("start i=",i,"\n")
	
	# For each cluster in cluster1
	for( j in list1)
	{
	
		cat("j ",j,"\n")
		# Verify and save true to another list
		# cat(cluster2@clusters[[i]] %in% cluster1@clusters[[j]])
		verify <- cluster2@clusters[[i]] %in% cluster1@clusters[[j]]
		cat(verify,"\n xxxx \n")
		tmp <- tmp2 <- verify[1]
		
		#check if all member of tmp are all true or all false.
		for( k in c(2:length(verify)))
		{
			#cat("k ",k,"\n")
			tmp <- tmp && verify[k]  #check all true
			tmp2 <- tmp2 || verify[k] #check all false
		}
		
		# if all true, then cluster i has no asynchronous break.
		if(tmp == TRUE)
		{
			cat("all true break\n")
			break
			
		}else{
		
			#cat("print 2 \n")
			# if there exist at lest one false 
			if(tmp2 != FALSE)
			{
				aList = list()
				aList[length(list1)+1] <- 9999
				cat("print 3 \n")
				for( m in c(1:length(verify)))
				{	
					cat("print ",m, verify[m] ,"\n")	
					if(isTRUE(verify[m]))
					{
						# create group and add it into
						cat("check ",cluster2@clusters[[i]][m],"\n") 
						aList[[j]][length(aList[[j]])+1] <- cluster2@clusters[[i]][m] 
						cat("1. aList[",j,"]", aList[[j]], "\n")
					}
				}
				
				for( m in c(1:length(verify)))
				{
					if(!isTRUE(verify[m])){
						# check with consecutive cluster1
						
						for( p in c(1:length(cluster1@clusters)))
						{
							cat("p = ",p,"\n")
							for( pp in c(1:length(cluster1@clusters[[p]])))
							{
								if(cluster2@clusters[[i]][m] == cluster1@clusters[[p]][pp])
								{
									aList[[p]][length(aList[[p]])+1] <- cluster2@clusters[[i]][m]
									cat("2. aList[",p,"]", aList[[p]], "\n")
									break;
								}
							}
						}
					}
				}
				
				cat("end ************************** \n")
				
				#process
				
				break;
			}
		}
	}
	#cat("finish")
	#cat("\n")
}


#nClus <- length(cluster@clusters)
#i_list<-c(1: nClus)
#write.table(measure, file = output, sep = " ", row.names=FALSE)
#write.table(asyncTable, file = async, sep = " ", row.names=FALSE)
