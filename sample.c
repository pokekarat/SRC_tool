//Ekarat Rattagan
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
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>

int main(int argc, char **argv)
{
    if(argc < 5)
    {
        printf("Please specify [sample rate (Hz)] [duration (secs)] [wifi-path] [packagename] , e.g., 1 100 /sys/class/net/wlan0/ com.google.android.youtube \n");
        exit(-1);
    }

    char wifiPath[1024];
    char packageName[1024];
    
    float sampleRate            =         atof(argv[1]);         //printf("sampleRate %f\n", sampleRate);
    float duration              =         atof(argv[2]); 	 //printf("duration %f\n", duration);
    strcpy(wifiPath, argv[3]);
    strcpy(packageName, argv[4]);

    float nrows                 =         duration * sampleRate;      //printf("nrows %f\n", nrows);
    int sleep                   =         (int)(1000000.0f / sampleRate);       // printf("sleep %d\n",sleep);
    
    char txPack[1024];
    char rxPack[1024];
    char txByte[1024];
    char rxByte[1024];

    sprintf(txPack,"%s/statistics/tx_packets",wifiPath);
    sprintf(rxPack,"%s/statistics/rx_packets",wifiPath);
    sprintf(txByte,"%s/statistics/tx_bytes",wifiPath);
    sprintf(rxByte,"%s/statistics/rx_bytes",wifiPath);

    //Create directory if it not exists
    struct stat st2 = {0};
    if(stat("/data/local/tmp/stat/", &st2) == -1){
        mkdir("/data/local/tmp/stat/",0777);
    }

    FILE *fp,
         *fp_proc,
         *fp3_app,
         *fp_uid_snd,
         *fp_uid_rcv,
         *fp_util,
         *fp_app_util,
         *fp_freq_0,
         *fp_freq_1,
         *fp_freq_2,
         *fp_freq_3,
         *fp_freq_4,
         *fp_freq_5,
         *fp_freq_6,
         *fp_freq_7,
         *fp_mem_total,
         *fp_mem_app,
         *fp_tx_pk,
         *fp_rx_pk,
         *fp_tx_byte,
         *fp_rx_byte,
 	 *fp_capacity,
         *fp_volt,
         *fp_temp;

    struct timeb t_start, t_current;
    struct stat st;

    long ms; // Milliseconds
    time_t s; // Seconds
    struct timespec spec;

    int i=0,t=0;
    int size = 1024;
    char buffer[size*1024],
        line[size * 10],
        uid_tmp[size],
        buffer_app[size*10],
        buffer_uid_snd[size],
        buffer_uid_rcv[size],
        buffer_tx_pk[size],
        buffer_rx_pk[size],
        buffer_tx_byte[size],
        buffer_rx_byte[size];

    char buffer_freq_0[size];
    char buffer_freq_1[size];
    char buffer_freq_2[size];
    char buffer_freq_3[size];
    char buffer_freq_4[size];
    char buffer_freq_5[size];
    char buffer_freq_6[size];
    char buffer_freq_7[size];

    char buffer_mem_total[size];
    char buffer_mem_app[size];
    char buffer_null[2];
    buffer_null[0] = '0';
    buffer_null[1] = '\n';

    char buffer_capacity[size];
    char buffer_volt[size];
    char buffer_temp[size];
   
    
    int ncolumns                 =         size;

    char aut[size];
    char freqs[size];
    int setSize = size * sizeof(char *);

    //define size of rows
    char **save_util             =         (char **)malloc(setSize);
    char **save_freq_0           =         (char **)malloc(setSize);
    char **save_freq_1           =         (char **)malloc(setSize);
    char **save_freq_2           =         (char **)malloc(setSize);
    char **save_freq_3           =         (char **)malloc(setSize);
    char **save_freq_4           =         (char **)malloc(setSize);
    char **save_freq_5           =         (char **)malloc(setSize);
    char **save_freq_6           =         (char **)malloc(setSize);
    char **save_freq_7           =         (char **)malloc(setSize);
    char **save_tx_pk            =         (char **)malloc(setSize);
    char **save_rx_pk            =         (char **)malloc(setSize);
    char **save_tx_byte          =         (char **)malloc(setSize);
    char **save_rx_byte          =         (char **)malloc(setSize);
    char **save_mem_total        =         (char **)malloc(setSize);
    char **save_capacity 	 = 	   (char **)malloc(setSize);
    char **save_volt 		 = 	   (char **)malloc(setSize);
    char **save_temp 		 = 	   (char **)malloc(setSize);
   
    char **save_mem_app          =         (char **)malloc(setSize);
    char **save_app              =         (char **)malloc(setSize);
    char **save_uid_snd          =         (char **)malloc(setSize);
    char **save_uid_rcv          =         (char **)malloc(setSize);
   
    char **multiproc 		 =         (char **)malloc(setSize);
    char **pids			 =	   (char **)malloc(setSize);
    char **uids			 =	   (char **)malloc(setSize);
    char **pid_stat		 =	   (char **)malloc(setSize);
    char **uid_stat		 =	   (char **)malloc(setSize);
    char **mem_stat		 =	   (char **)malloc(setSize);
    
    float xx;
    int x = 0;
    int nSize = ncolumns * sizeof(char);
    for(xx=0.0; xx < nrows; xx++)
    {
       
		save_util[x]                   =         (char *)malloc(nSize);
		save_freq_0[x]                 =         (char *)malloc(nSize);
		save_freq_1[x]                 =         (char *)malloc(nSize);
		save_freq_2[x]                 =         (char *)malloc(nSize);
		save_freq_3[x]                 =         (char *)malloc(nSize);
		save_freq_4[x]                 =         (char *)malloc(nSize);
		save_freq_5[x]                 =         (char *)malloc(nSize);
		save_freq_6[x]                 =         (char *)malloc(nSize);
		save_freq_7[x]                 =         (char *)malloc(nSize);
		save_mem_total[x]              =         (char *)malloc(nSize);
		save_tx_pk[x]                  =         (char *)malloc(nSize);
		save_tx_byte[x]                =         (char *)malloc(nSize);
		save_rx_pk[x]                  =         (char *)malloc(nSize);
		save_rx_byte[x]                =         (char *)malloc(nSize);
		save_capacity[x] 	       = 	 (char *)malloc(nSize);
 	        save_volt[x] 		       = 	 (char *)malloc(nSize);
		save_temp[x] 		       = 	 (char *)malloc(nSize);

	
	 	save_mem_app[x]                =         (char *)malloc(nSize);
		save_app[x]                    =         (char *)malloc(nSize);
		save_uid_snd[x]                =         (char *)malloc(nSize);
		save_uid_rcv[x]                =         (char *)malloc(nSize);
	
		multiproc[x]                   =         (char *)malloc(nSize);
		pids[x]                        =         (char *)malloc(nSize);
	        uids[x]                        =         (char *)malloc(nSize);
		pid_stat[x]                    =         (char *)malloc(nSize);
		mem_stat[x]                    =         (char *)malloc(nSize);
		uid_stat[x]		       =	 (char *)malloc(nSize);
        	//printf("x = %d, xx= %f \n", x, xx);
        	++x;
    }

    char uid_stat_snd[size];
    char uid_stat_rcv[size];
    int pidLen = 0;
    char *n = NULL;
    char *nn = NULL;
    int firstTime = 1;
    int firstTime2 = 1;
    int f=0;
    char * pch;
    char * pch2;

    printf("start sample..\n");

    int nProcs = 0;
    char appTest[1024];
    while(i < nrows)
    {
 		// >> for append, > for overwrite.
		
		sprintf(aut,"ps | /data/local/tmp/busybox grep %s > /data/local/tmp/tmp_aut", packageName);
                system(aut);
	
		fp_proc = fopen("/data/local/tmp/tmp_aut", "r");
		if(fp_proc != NULL)
		{
			//printf("open tmp_aut file\n");
			nProcs = 0;
                        while(fgets(line, sizeof line, fp_proc) != NULL)
			{
	                        printf("line %d = %s\n", nProcs, line);
				strcpy(multiproc[nProcs], line);
				++nProcs;
			}
		        fclose(fp_proc);
                        

			printf("nProcs = %d\n",nProcs);
			int r;
			
			char index[1024];
			sprintf(index,"\n%d+++\n",i);
			strcpy(save_uid_snd[i],index);
			strcpy(save_uid_rcv[i],index);
			strcpy(save_app[i],index);
			strcpy(save_mem_app[i],index);

			for(r=0; r<nProcs; r++)
			{
				//printf("process = %s\n", multiproc[r]);
				//printf("Split line\n");
				pch = strtok(multiproc[r], " "); // split string into tokens

				//UID /////////////////////////////
				if(pch != NULL) // get 1st token to check is it uid.
				{
				        //printf("Extract uid\n");
				        strtok_r(pch, "a", &pch2);
					//printf("pch2 %s \n",pch2);

					int _uid = 0;
					if(pch2 != NULL)
					{
					        strcpy(uids[r],pch2);
						_uid = 10000 + atoi(uids[r]);
					}else{
						_uid = 0;
					}
					
				        sprintf(uids[r], "%d", _uid);
					//printf("uids[%d] = %s \n",r,uids[r]);
				} 


				//Assign UID data
				nn = malloc(strlen( uids[r]? uids[r] : "\n" ));

				if(uids[r])
				    strcpy(nn, uids[r]);

				int snd = sprintf(uid_stat_snd, "/proc/uid_stat/%s/tcp_snd",nn);
				int rcv = sprintf(uid_stat_rcv, "/proc/uid_stat/%s/tcp_rcv",nn);

				//printf("%s \n", uid_stat_snd);			 	
 				if((fp_uid_snd = fopen(uid_stat_snd,"r")) != NULL)
				{
					//printf("Save uid_stat_snd\n");
					fgets(buffer_uid_snd, sizeof buffer_uid_snd, fp_uid_snd);
					//printf("Save uid_stat_snd2 %s \n", buffer_uid_snd);
					strcat(save_uid_snd[i], buffer_uid_snd);
					//printf("save_uid_snd[%d][%d] %s \n",i,r, save_uid_snd[i]);
					buffer_uid_snd[0] = '\0';
					fclose(fp_uid_snd);
					//printf("End uid_stat_snd\n");

				}
				else
				{
			    		//strcat(save_uid_snd[i], "0   ");
		    			strcat(save_uid_snd[i], buffer_null);
			   	}

				//strcat(save_uid_snd[i], "\n+++\n");

				//printf("%s \n", uid_stat_rcv);
				if((fp_uid_rcv = fopen(uid_stat_rcv,"r")) != NULL)
				{
					fgets(buffer_uid_rcv, sizeof buffer_uid_rcv, fp_uid_rcv);
					strcat(save_uid_rcv[i], buffer_uid_rcv);
					buffer_uid_rcv[0] = '\0';
					fclose(fp_uid_rcv);
				}
				else
				{
			    		//strcat(save_uid_rcv[i], "0   ");
		    			strcat(save_uid_rcv[i], buffer_null);
				}

				//strcat(save_uid_rcv[i], "\n+++\n");

				///////////////////////////////////////PID ////////////////////////////////////////////    
				pch = strtok(NULL, " "); // move to 2nd token, process id
				if(pch != NULL)
					strcpy(pids[r], pch);
				else
					printf("Error pid\n");

				//printf("pid[%d] = %s \n",r, pids[r]);

				n = malloc(strlen( pids[r]? pids[r] : "\n" ));
   			        strcpy(n, pids[r]);

				sprintf(pid_stat[r], "/proc/%s/stat",n);	
				if((fp3_app = fopen(pid_stat[r],"r")) != NULL)
				{
					fgets(buffer_app, sizeof buffer_app, fp3_app);
					strcat(save_app[i], buffer_app);
					buffer_app[0] = '\0';
					fclose(fp3_app);
					//printf("3 app util %s \n",save_app[i]);
				}
				else
				{
		    			strcat(save_app[i], buffer_null);
		    			//buffer_null[0] = '\0';
				}
				
				//strcat(save_app[i], "\n+++\n");			

			        sprintf(mem_stat[r], "/proc/%s/statm",n);
				if((fp_mem_app = fopen(mem_stat[r],"r")) != NULL)
			        {
					//mem app
					fgets(buffer_mem_app, sizeof buffer_mem_app, fp_mem_app);
					strcat(save_mem_app[i], buffer_mem_app);
					buffer_mem_app[0] = '\0';
					fclose(fp_mem_app);
					//printf("4 app mem util %s \n",save_mem_app[i]);
				}
				else
				{
		    			strcat(save_mem_app[i], buffer_null);
		    			//buffer_null[0] = '\0';
				}
				//strcat(save_mem_app[i], "\n+++\n");
			}
		} // End if package length.

		//total cpu need to sample on both modes
		///////////////////////////////////// utilization /////////////////////////////////////////
		if((fp = fopen("/proc/stat","r")) != NULL)
		{
		    char a[9999], b[9999];
		    int c=0;
		    for(c=0; c<=7; c++)
		    {
		        fgets(buffer,sizeof buffer,fp);
		        if(buffer[0] != 'c' ) continue;
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
	 	
		//////////////////////////////// frequency 0 /////////////////////////////////////////////
		if((fp_freq_0 = fopen("/sys/devices/system/cpu/cpu0/cpufreq/scaling_cur_freq","r")) != NULL)
		{
		    fgets(buffer_freq_0, sizeof buffer_freq_0, fp_freq_0);
		    strcpy(save_freq_0[i], buffer_freq_0);
		    buffer_freq_0[0] = '\0';
		    //printf("7.1 freq %s \n",save_freq[i]);
		    fclose(fp_freq_0);
		    //printf("%d",i);
		}
		else
		{
		    strcpy(save_freq_0[i], buffer_null);
		 }

		//////////////////////////////// frequency 1 ///////////////////////////////////////////////////
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
		    strcpy(save_freq_1[i], buffer_null);
		   
		}

		 //////////////////////////////// frequency 2 ///////////////////////////////////////////////////
		if((fp_freq_2 = fopen("/sys/devices/system/cpu/cpu2/cpufreq/scaling_cur_freq","r")) != NULL)
		{
		    fgets(buffer_freq_2, sizeof buffer_freq_2, fp_freq_2);
		    strcpy(save_freq_2[i], buffer_freq_2);
		    buffer_freq_2[0] = '\0';
		    fclose(fp_freq_2);
		}
		else
		{
		    strcpy(save_freq_2[i], buffer_null);
		 }

		//////////////////////////////// frequency 3 ///////////////////////////////////////////////////
		if((fp_freq_3 = fopen("/sys/devices/system/cpu/cpu3/cpufreq/scaling_cur_freq","r")) != NULL)
		{
		    fgets(buffer_freq_3, sizeof buffer_freq_3, fp_freq_3);
		    strcpy(save_freq_3[i], buffer_freq_3);
		    buffer_freq_3[0] = '\0';
		    fclose(fp_freq_3);
		}
		else
		{
		   
		    strcpy(save_freq_3[i], buffer_null);
		   
		}

		//////////////////////////////// frequency 4 ///////////////////////////////////////////////////
		if((fp_freq_4 = fopen("/sys/devices/system/cpu/cpu4/cpufreq/scaling_cur_freq","r")) != NULL)
		{
		    fgets(buffer_freq_4, sizeof buffer_freq_4, fp_freq_4);
		    strcpy(save_freq_4[i], buffer_freq_4);
		    buffer_freq_4[0] = '\0';
		    fclose(fp_freq_4);
		}
		else
		{
		   
		    strcpy(save_freq_4[i], buffer_null);
		  
		}

		//////////////////////////////// frequency 5 ///////////////////////////////////////////////////
		if((fp_freq_5 = fopen("/sys/devices/system/cpu/cpu5/cpufreq/scaling_cur_freq","r")) != NULL)
		{
		    fgets(buffer_freq_5, sizeof buffer_freq_5, fp_freq_5);
		    strcpy(save_freq_5[i], buffer_freq_5);
		    buffer_freq_5[0] = '\0';
		    fclose(fp_freq_5);
		}
		else
		{
		   
		    strcpy(save_freq_5[i], buffer_null);
		  
		}

		//////////////////////////////// frequency 6 ///////////////////////////////////////////////////
		if((fp_freq_6 = fopen("/sys/devices/system/cpu/cpu6/cpufreq/scaling_cur_freq","r")) != NULL)
		{
		    fgets(buffer_freq_6, sizeof buffer_freq_6, fp_freq_6);
		    strcpy(save_freq_6[i], buffer_freq_6);
		    buffer_freq_6[0] = '\0';
		    fclose(fp_freq_6);
		}
		else
		{
		   
		    strcpy(save_freq_6[i], buffer_null);
		   
		}

		//////////////////////////////// frequency 7 ///////////////////////////////////////////////////
		if((fp_freq_7 = fopen("/sys/devices/system/cpu/cpu7/cpufreq/scaling_cur_freq","r")) != NULL)
		{
		    fgets(buffer_freq_7, sizeof buffer_freq_7, fp_freq_7);
		    strcpy(save_freq_7[i], buffer_freq_7);
		    buffer_freq_7[0] = '\0';
		    fclose(fp_freq_7);
		}
		else
		{
		   
		    strcpy(save_freq_7[i], buffer_null);
		   
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

		/////////////////////////////// Wi-Fi tx packets //////////////////////////////////////////////
		if((fp_tx_pk = fopen(txPack,"r")) != NULL)
		{
		    fgets(buffer_tx_pk, sizeof buffer_tx_pk, fp_tx_pk);
		    strcpy(save_tx_pk[i], buffer_tx_pk);
		    buffer_tx_pk[0] = '\0';
		    fclose(fp_tx_pk);
		}
		else
		{
		   
		    strcpy(save_tx_pk[i], buffer_null);
		   
		}

		/// Wi-Fi rx packets
		if((fp_rx_pk = fopen(rxPack,"r")) != NULL)
		{
		    fgets(buffer_rx_pk, sizeof buffer_rx_pk, fp_rx_pk);
		    strcpy(save_rx_pk[i], buffer_rx_pk);
		    buffer_rx_pk[0] = '\0';
		    fclose(fp_rx_pk);
		}
		else
		{
		  
		    strcpy(save_rx_pk[i], buffer_null);
		  
		}

		/// Wi-Fi tx bytes
		if((fp_tx_byte = fopen(txByte,"r")) != NULL)
		{
		    fgets(buffer_tx_byte, sizeof buffer_tx_byte, fp_tx_byte);
		    strcpy(save_tx_byte[i], buffer_tx_byte);
		    buffer_tx_byte[0] = '\0';
		    fclose(fp_tx_byte);
		}
		else
		{
		   
		    strcpy(save_tx_byte[i], buffer_null);
		   
		}

		/// Wi-Fi rx bytes
		if((fp_rx_byte = fopen(rxByte,"r")) != NULL)
		{
		    fgets(buffer_rx_byte, sizeof buffer_rx_byte, fp_rx_byte);
		    strcpy(save_rx_byte[i], buffer_rx_byte);
		    buffer_rx_byte[0] = '\0';
		    fclose(fp_rx_byte);
		}
		else
		{
		
		    strcpy(save_rx_byte[i], buffer_null);
		   
		}

	       //Battery
	       ////////////////////////////////////// temperature /////////////////////////////////////////////////
	       if((fp_temp = fopen("/sys/class/power_supply/battery/temp","r")) != NULL){

		    fgets(buffer_temp, sizeof buffer_temp, fp_temp);
		    strcpy(save_temp[i], buffer_temp);
		    buffer_temp[0] = '\0';
		    //printf("9.1 temperature %s \n",save_temp[i]);
		    fclose(fp_temp);

		}
                else printf("null\n");

	       ////////////////////////////////////// voltage /////////////////////////////////////////////////
	       if((fp_volt = fopen("/sys/class/power_supply/battery/voltage_now","r")) != NULL){

		    fgets(buffer_volt, sizeof buffer_volt, fp_volt);
		    strcpy(save_volt[i], buffer_volt);
		    buffer_volt[0] = '\0';
		    //printf("9 volt %s \n",save_volt[i]);
		    fclose(fp_volt);

		}

		////////////////////////////////////// capacity ////////////////////////////////////////////////
		if((fp_capacity = fopen("/sys/class/power_supply/battery/capacity","r")) != NULL){

		    fgets(buffer_capacity, sizeof buffer_capacity, fp_capacity);
		    strcpy(save_capacity[i], buffer_capacity);
		    buffer_capacity[0] = '\0';
		    //printf("10 capacity %s \n",save_capacity[i]);
		    fclose(fp_capacity);

		}

        	usleep(sleep); // ms * 1000
        	++i;
        	printf("loop %d\n",i);
         }
	
    	 printf("Start logging\n");

         printf("Initial log files\n");
    	 fp_util = fopen("/data/local/tmp/stat/cpu_util.txt", "w");
	 fp_freq_0 = fopen("/data/local/tmp/stat/freq0.txt", "w");
	 fp_freq_1 = fopen("/data/local/tmp/stat/freq1.txt", "w");
	 fp_freq_2 = fopen("/data/local/tmp/stat/freq2.txt", "w");
	 fp_freq_3 = fopen("/data/local/tmp/stat/freq3.txt", "w");
	 fp_freq_4 = fopen("/data/local/tmp/stat/freq4.txt", "w");
	 fp_freq_5 = fopen("/data/local/tmp/stat/freq5.txt", "w");
	 fp_freq_6 = fopen("/data/local/tmp/stat/freq6.txt", "w");
	 fp_freq_7 = fopen("/data/local/tmp/stat/freq7.txt", "w");
	 
	 fp_mem_total = fopen("/data/local/tmp/stat/mem_total.txt", "w");

	 fp_tx_pk = fopen("/data/local/tmp/stat/wifi_tx_pk.txt", "w");
	 fp_rx_pk = fopen("/data/local/tmp/stat/wifi_rx_pk.txt", "w");
	 fp_tx_byte = fopen("/data/local/tmp/stat/wifi_tx_byte.txt", "w");
	 fp_rx_byte = fopen("/data/local/tmp/stat/wifi_rx_byte.txt", "w");

	 fp_capacity = fopen("/data/local/tmp/stat/capacity.txt","w");
	 fp_volt = fopen("/data/local/tmp/stat/volt.txt","w");
	 fp_temp = fopen("/data/local/tmp/stat/temperature.txt","w");
   
	 int ii = 0;
	 int ff = 0;
	    
         printf("Saving training logs\n");
	 for(ii = 0; ii <= i-1; ii++)
	 {

		fprintf(fp_util,"%s "           ,         save_util[ii]);
		fprintf(fp_freq_0,"%s "         ,         save_freq_0[ii]);
		fprintf(fp_freq_1,"%s "         ,         save_freq_1[ii]); 
		fprintf(fp_freq_2,"%s "         ,         save_freq_2[ii]);
		fprintf(fp_freq_3,"%s "         ,         save_freq_3[ii]);
		fprintf(fp_freq_4,"%s "         ,         save_freq_4[ii]);
		fprintf(fp_freq_5,"%s "         ,         save_freq_5[ii]);
		fprintf(fp_freq_6,"%s "         ,         save_freq_6[ii]);
		fprintf(fp_freq_7,"%s "         ,         save_freq_7[ii]);
	       
		fprintf(fp_mem_total,"%s "      ,         save_mem_total[ii]);
		fprintf(fp_tx_pk,"%s "          ,         save_tx_pk[ii]);
		fprintf(fp_rx_pk,"%s "          ,         save_rx_pk[ii]);
		fprintf(fp_tx_byte,"%s "        ,         save_tx_byte[ii]);
		fprintf(fp_rx_byte,"%s "        ,         save_rx_byte[ii]);

		//battery
		fprintf(fp_capacity,"%s "       ,     	save_capacity[ii]);
		fprintf(fp_volt,"%s "		, 	save_volt[ii]);
		fprintf(fp_temp,"%s "		, 	save_temp[ii]);
	    }

	 n = NULL;
	 nn = NULL;
	 free(save_util);
	 free(save_freq_0);
	 free(save_freq_1);
	 free(save_freq_2);
	 free(save_freq_3);
	 free(save_freq_4);
	 free(save_freq_5);
	 free(save_freq_6);
	 free(save_freq_7);
	 free(save_mem_total);
	 free(save_tx_pk);
	 free(save_rx_pk);
	 free(save_tx_byte);
	 free(save_rx_byte);	
	 fclose(fp_temp);
	 fclose(fp_volt);
	 fclose(fp_capacity);  

 
	 fp_app_util = fopen("/data/local/tmp/stat/cpu_app.txt","w");
	 fp_uid_snd = fopen("/data/local/tmp/stat/uid_snd.txt","w");
	 fp_uid_rcv = fopen("/data/local/tmp/stat/uid_rcv.txt","w");
	 fp_mem_app = fopen("/data/local/tmp/stat/mem_app.txt", "w");

	 int b=0;
	 float bb;
	 int jj = 0;
	  
         printf("Saving testing logs\n"); 
	 for(jj = 0; jj<= i-1; jj++)
	 {
		fprintf(fp_app_util,"%s "         ,         save_app[jj]);
		fprintf(fp_uid_snd, "%s "         ,         save_uid_snd[jj]);
		fprintf(fp_uid_rcv,"%s "         ,         save_uid_rcv[jj]);
		fprintf(fp_mem_app,"%s "         ,         save_mem_app[jj]);

	 }
	      
	 free(save_app);
	 free(save_uid_snd);
	 free(save_uid_rcv);
	 free(save_mem_app);

	 system("rm /data/local/tmp/tmp_aut");

	 printf("finish logging\n");

	 return(0);
	
}
