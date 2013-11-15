1. Install R 2.15.3 from this website http://cran.r-project.org/bin/windows/base/old/2.15.3/
2. Set path to the bin folder of R 2.15.3 in order to let R program visible to every path
3. Test if path is set correctly by run RScript in any path in cmd
4. If success, you can run asynComponent.r as
	4.1 C:\your path\Rscript asynComponent.r [path]
	4.2 [path] is the path which contain your sample.txt obtained from sample process
5. If program finishes , it will print "finish: modifyPower.txt asyncTable.txt are saved"
6. You can check these two output files whether there exist in [path]