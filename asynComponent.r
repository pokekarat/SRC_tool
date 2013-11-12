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

i_list <- c(1:length(cluster2@clusters))
j_list <- c(1:length(cluster1@clusters))

for(i in i_list)
{
	cat("start") 
	cat("\n")
	for( j in j_list)
	{	
		cat(cluster2@clusters[[i]] %in% cluster1@clusters[[j]])
		cat("\n")
	}
	cat("finish")
	cat("\n")
}


#nClus <- length(cluster@clusters)
#i_list<-c(1: nClus)
#write.table(measure, file = output, sep = " ", row.names=FALSE)
#write.table(asyncTable, file = async, sep = " ", row.names=FALSE)
