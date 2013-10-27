#define _POSIX_C_SOURCE 200809L

#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>
#include <sys/time.h>
#include <string.h>
#include <sys/timeb.h>
#include <sys/stat.h>
#include <inttypes.h>

int main(int argc, char **argv)
{
    if(argc < 3)
    {
        printf("Please specify [delay time (sec), number of loops, packagename], e.g., 1 100 com.snctln.game.BreakTheBlocks \n"); 
        exit(-1);
    }

    //printf("argc %d",argc);
    
    FILE *fp, *fp2, *fp3_app, *fp_util,
         *fp_app_util,
         *fp_freq,
         *fp_freq_1,
         *fp_mem_total,
         *fp_mem_app;
      
    struct timeb t_start, t_current;
    struct stat st;

    long            ms; // Milliseconds
    time_t          s;  // Seconds
    struct timespec spec;

    int i=0,j=1,t=0;
    int size = 1024;
    char buffer[size*1024], appId[size], buffer_app[size*10];
 
    char buffer_freq[size];
    char buffer_freq_1[size];
    char buffer_mem_total[size];
    char buffer_mem_app[size];
    char buffer_freq_null[2];
    char buffer_app_null[2];

    float delay1    		= 	atof(argv[1]);        	//printf("delay1 %f\n", delay1);
    float delay     		= 	atof(argv[1]);        	//printf("delay %f\n", delay);
    float end	    		= 	atof(argv[2]);          //printf("end %f\n", end);
    float nrows     		= 	end/delay;      	//printf("nrows %f\n", nrows);
    float step      		= 	delay1;               	//printf("step %f\n", step);
    char filename[100];
    
    //Add the package name of the target AUT. 
    if(argc==4)
	    strcpy(filename, argv[3]);
    
    char aut[100];
    char freqs[100];

    int ncolumns 		= 	size;
    int isKilled 		= 	1;
    int sleep 			= 	(int)(delay * 990000.0f);	printf("sleep %d\n",sleep);
    int r 			= 	1024;

    char **save_util 		= 	(char **)malloc(r * 1024 * sizeof(char *));
    char **save_freq 		= 	(char **)malloc(r * sizeof(char *));
    char **save_freq_1 		= 	(char **)malloc(r * sizeof(char *));
    char **save_mem_total 	= 	(char **)malloc(r * 10 * sizeof(char *));
    char **save_mem_app 	= 	(char **)malloc(r * 10 * sizeof(char *));
    char **save_app 		= 	(char **)malloc(r * 1024 * sizeof(char *));

    float xx;
    int x = 0;

    //printf("allocate array\n");    
    for(xx=0.0; xx < nrows; xx++)
    {
        save_util[x] 		= 	(char *)malloc(ncolumns * sizeof(char));
        save_freq[x] 		= 	(char *)malloc(ncolumns * sizeof(char));
        save_freq_1[x] 		= 	(char *)malloc(ncolumns * sizeof(char));
        save_mem_total[x] 	= 	(char *)malloc(ncolumns * sizeof(char));
        save_mem_app[x] 	= 	(char *)malloc(ncolumns * sizeof(char));
        save_app[x] 		= 	(char *)malloc(ncolumns * sizeof(char));
        //printf("x = %d, xx= %f \n", x, xx);
	++x;
    }

    char appName[size];
    char memName[size];
    int len = 0;
    char *n = NULL;
    int firstTime = 1;
    int f=0;

    for(;;)
    {
        if(len == 0)
        {
            sprintf(aut, "busybox pidof %s > /dev/null", filename);
            if(system(aut) == 0)
            {
                sprintf(aut,"busybox pidof %s >> /data/local/tmp/tmp_aut",filename);
                system(aut);
                fp2 = fopen("/data/local/tmp/tmp_aut","r");
                fgets(appId, sizeof appId, fp2);
                fclose(fp2);
                len = strlen(appId);
             	//printf("1, PID length = %d\n",len);
                system("rm /data/local/tmp/tmp_aut");
            }
        }

        if(len != 0)
        {
           if(firstTime)
           {
                firstTime = 0;
                n = malloc( strlen( appId? appId : "\n" ) );
                if( appId )
                    strcpy( n, appId );
                n[ strlen( n )-1 ] = '\0';
                // Assign appName with specific pid
                int c = sprintf(appName, "/proc/%s/stat",n);
             	//printf("2, App name = %s\n",appName);
                int m = sprintf(memName, "/proc/%s/statm",n);
                j=i;
            }

            if((fp3_app = fopen(appName,"r")) != NULL)
            {
                fgets(buffer_app,sizeof buffer_app,fp3_app);
                strcpy(save_app[i], buffer_app);
                buffer_app[0] = '\0';
                fclose(fp3_app);
              	//printf("3 util app %s \n",save_app[i]);
            }
	    else //if AUT is not launched yet
	    {
	    	buffer_app_null[0] = '0';
	    	buffer_app_null[1] = '\n';
            	strcpy(save_app[i], buffer_app_null);
            	buffer_app_null[0] = '\0';
	    }

            if((fp_mem_app = fopen(memName,"r")) != NULL)
            {
                //mem app
                fgets(buffer_mem_app,sizeof buffer_mem_app,fp_mem_app);
                strcpy(save_mem_app[i],buffer_mem_app);
                buffer_mem_app[0] = '\0';
                fclose(fp_mem_app);
                //printf("4 mem usage app %s \n",save_mem_app[i]);
           }
        }
        else //if AUT is not launched yet
	{
	    buffer_app_null[0] = '0';
	    buffer_app_null[1] = '\n';
            strcpy(save_app[i], buffer_app_null);
            buffer_app_null[0] = '\0';
	}

        //////////////////////////// mem (total) /////////////////////////////////////////////////////
        if((fp_mem_total = fopen("/proc/meminfo","r")) != NULL)
        {
                int count = 0;
                while(fgets(buffer_mem_total, sizeof buffer_mem_total, fp_mem_total) != NULL)
                {
                    strcat(save_mem_total[i],buffer_mem_total);
                    ++count;
                    if(count == 2) break;
                }
                count = 0;
                buffer_mem_total[0] = '\0';
               	//printf("5 mem total %s \n",save_mem_total[i]);
                fclose(fp_mem_total);
        }
        
        /////////////////////////////////////  utilization  /////////////////////////////////////////
        if((fp = fopen("/proc/stat","r")) != NULL) 
	{
            char a[9999], b[9999];
	    int r=1;
            for(r=1; r<=3; r++)
	    {
		fgets(buffer,sizeof buffer,fp);
                if(buffer[0] == 'i') continue;
                //printf("6. %s\n", buffer);
	        strcat(a, buffer);
                //printf("6.1 %s\n",a);
		buffer[0] = '\0';
	    }

	    strcpy(save_util[i],a);  
            buffer[0] = '\0';
            a[0] = '\0';
	    //printf("6 util %s \n",save_util[i]);
            fclose(fp);
        }


        //////////////////////////////// frequency 1 /////////////////////////////////////////////
  	
	if((fp_freq = fopen("/sys/devices/system/cpu/cpu0/cpufreq/scaling_cur_freq","r")) != NULL)
        {
            fgets(buffer_freq, sizeof buffer_freq, fp_freq);
            strcpy(save_freq[i], buffer_freq);
            buffer_freq[0] = '\0';
            //printf("7.1 freq %s \n",save_freq[i]);
            fclose(fp_freq);
	    //printf("%d",i);
        }
	else
	{
            buffer_freq_null[0] = '0';
            strcpy(save_freq[i], buffer_freq_null);
            buffer_freq_null[0] = '\0';
        }

        //////////////////////////////// frequency 2 ///////////////////////////////////////////////////
        if((fp_freq_1 = fopen("/sys/devices/system/cpu/cpu1/cpufreq/scaling_cur_freq","r")) != NULL)
        {
            fgets(buffer_freq_1, sizeof buffer_freq_1, fp_freq_1);
            strcpy(save_freq_1[i], buffer_freq_1);
            buffer_freq_1[0] = '\0';
            //printf("7.2 freq %s \n",save_freq_1[i]);
            fclose(fp_freq_1);
        }
        else
        {
            buffer_freq_null[0] = '0';
 	    buffer_freq_null[1] = '\n';
            strcpy(save_freq_1[i], buffer_freq_null);
            buffer_freq_null[0] = '\0';
        }

        usleep(sleep); // ms * 1000
        delay1 += step;

        //printf("sleep = %d, delay1 = %f, i = %d, end = %f \n",timeStamp, sleep, delay1, i, end);
	
	clock_gettime(CLOCK_REALTIME, &spec);
    	s  = spec.tv_sec;
    	ms = round(spec.tv_nsec / 1.0e6); // Convert nanoseconds to milliseconds
    	
	//printf("Current time: %"PRIdMAX".%03ld seconds since the Epoch\n", (intmax_t)s, ms);
        
	++i;

        if(delay1 > end) // save at the last time
        {
            //printf("end\n");
            fp_util = fopen("/data/local/tmp/stat/cpu_util.txt", "w");
            fp_freq = fopen("/data/local/tmp/stat/freq.txt", "w");
            fp_freq_1 = fopen("/data/local/tmp/stat/freq1.txt", "w");
	    fp_mem_app = fopen("/data/local/tmp/stat/mem_app.txt", "w");
            fp_mem_total = fopen("/data/local/tmp/stat/mem_total.txt", "w");
           
	    int ii = 0;
            //printf("i = %d\n",i);
	    int ff = 0;
            for(ii = 0; ii <= i-1; ii++)
	    {
                fprintf(fp_util,"%s "    	,     	save_util[ii]);
   		fprintf(fp_freq,"%s "    	,     	save_freq[ii]);
                fprintf(fp_freq_1,"%s "    	,     	save_freq_1[ii]);     
		fprintf(fp_mem_app,"%s "    	,     	save_mem_app[ii]);
		fprintf(fp_mem_total,"%s "    	,     	save_mem_total[ii]);                     
		
		/*for(ff=0; ff<=1; ff++)
		{
			fprintf(fp_freq,"%s "    	,     	save_freq[ii][ff]);
                	fprintf(fp_freq_1,"%s "    	,     	save_freq_1[ii][ff]);
                }*/
		//printf("ii=%i\n",ii);
            }

            fp_app_util = fopen("/data/local/tmp/stat/cpu_app.txt","w");
            int b=0;
	    float bb;
            int jj = 0;
           
            for(jj = 0; jj<= i-1; jj++)
            {
                fprintf(fp_app_util,"%s "    	,     	save_app[jj]);
            }
      	    
	    free(save_app);
            fclose(fp_app_util);

            n = NULL;
            free(save_util);
            free(save_freq);
 	    free(save_mem_app);
 	    free(save_mem_total);
           
            fclose(fp_util);
            fclose(fp_freq);
            fclose(fp_freq_1);
            fclose(fp_mem_app);
 	    fclose(fp_mem_total);
            
	    printf("save app\n");

            exit(-1);
        }
    }

    return(0);
}

