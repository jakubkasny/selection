clc, clear, close all
%% Read data
id = '083';
xRunId = ['h',id];
% read parameters:
fileFolder = ['D:\Kasa\prace\Turbulence_new\h-runs\',id,'\'];
%fileName = 'h088_m09_HeTWiCa_2022-10-10.h5';
fileName = 'h083_m09_HeTWiCa_2022-10-03.h5';

[data, ~] = read_hd5([],[],fileFolder, fileName, '','','');
%data.computeChanns

%read data:
actualChan = data.computeChanns;
method = 'raw/';
parameters = [];

[data, res] = read_hd5(data,parameters,fileFolder, fileName,actualChan,method,[],0,0);
%figure
%plot(data.allData{1,1}.data(:,1))
if res ~= 1
    disp("There was some mistake during reading, the program will proceed, but be aware.")
end

% read working data:
indexes = [3:6,9:12];
%indexes = [1:6,9:14];

xlabs = data.allData{1,1}.label(indexes);
xFreqB = data.allData{1,1}.srate;

dataBackup = data.allData{1,1}.data(:,indexes);

clear actualChan fileName method parameters res data

% cut:
dataBackup = dataBackup(2e5:end-2e5-1,:);

% reformat data
for ix = 1:length(indexes)
    if mod(ix,2)==0
        labs(1,ix*0.5,2) = xlabs(:,ix);
        data(:,ix*0.5,2) = dataBackup(:,ix)-mean(dataBackup(:,ix));
        data(:,ix*0.5,2) = data(:,ix*0.5,2)/std(data(:,ix*0.5,2));
    else
        labs(:,ceil(ix*0.5),1) = xlabs(:,ix);
        data(:,ceil(ix*0.5),1) = dataBackup(:,ix)-mean(dataBackup(:,ix));
        data(:,ceil(ix*0.5),1) = data(:,ceil(ix*0.5),1)/std(data(:,ceil(ix*0.5),1));
    end
end
dataInfered = data;
%data(:,1,:) = -data(:,1,:);
n = length(indexes)/2;

%% Read cell and material properties
importProperties
%% Auto interfering frequencies finder - double version

% Auto interfering frequencies finder
%data = dataInfered;
figure

for ir = [1,2]
    for ix = 1:n
        if ix == 4
            bp = 1;
        end
        
        bw = 0.015;
        dataH = dataInfered(:,ix,ir);
        dataH = dataH/std(dataH);
        
        % SUBPLOT: data:
        subplot(7,n,ix)
        dat = dataInfered(:,ix,ir) / std(dataInfered(:,ix,ir));
        plot(dat)
        title(labs(:,ix,ir))
        
        % SUBPLOT: spectrum - no bandpass
        subplot(7,n,n+ix)
        [ps, f] =  pspectrum(dataH);
        maxSpec = max(ps);
        %plot(f/pi*xFreqB/2,ps)
        findpeaks(log(ps),'MinPeakProminence',maxSpec)
        set(gca, 'XScale', 'log')
        %set(gca, 'YScale', 'log')
        grid on
            
        % SUBPLOT: AC
        subplot(7,n,2*n+ix)
        [det,~,~] = detrendMovMean(dataH,44*xFreqB*tff);
        [ac, lag ] = xcorr(det,'normalized');
        lag = lag/xFreqB;
        plot(lag, ac)
        xlabel('[s]')
        xlim([-5*tff, 5*tff])
        ylim([0.3 1.1])
        
        for rep = 1:2
            
            [ps, f] =  pspectrum(dataH);
            fTrans = f/pi*xFreqB/2;
            psTrans = ps;
        
            maxSpec = max(ps);
            
            [~, loc, ~, ~] = findpeaks(log(psTrans),'MinPeakProminence',maxSpec/rep);
            
            while ~isempty(loc)
                peak = fTrans(loc(end)); % ii(end) - vezme peak s nejvetsi prominenci
                dataH = bandstop(dataH,[peak-bw/2 peak+bw/2],xFreqB);
                
                loc = loc(1:end-1);
            end
            dataH = dataH/std(dataH);
            
            % SUBPLOT: spectrum - 1st and 2nd filtering
            subplot(7,n,(rep*2+1)*n+ix)
            [ps, f] =  pspectrum(dataH);
            maxSpec = max(ps);
            % SUBPLOT: plot(f/pi*xFreqB/2,ps)
            findpeaks(log(ps),'MinPeakProminence',maxSpec)
            set(gca, 'XScale', 'log')
            %set(gca, 'YScale', 'log')
            grid on
            
            % SUBPLOT: AC after first and second filtering
            subplot(7,n,(rep*2+2)*n+ix)
            [det,~,~] = detrendMovMean(dataH,44*xFreqB*tff);
            [ac, lag ] = xcorr(det,'normalized');
            lag = lag/xFreqB;
            plot(lag, ac)
            xlabel('[s]')
            xlim([-5*tff, 5*tff])
            ylim([0.3 1.1])
            
        end
        data(:,ix,ir) = dataH;
    end
end

%%
% Check signals
ir = 1;

figure

for ix = 1:n    
    subplot(4,n,ix)
    dat = dataInfered(:,ix,ir) / std(dataInfered(:,ix,ir));
    plot(dat)
    title(labs(:,ix,ir))
    
    subplot(4,n,n+ix)
    [ps, f] =  pspectrum(dat);
    plot(f/pi*xFreqB/2,ps)
    set(gca, 'XScale', 'log')
    set(gca, 'YScale', 'log')
end 
sgtitle(xRunId) 

% Auto interfering frequencies finder
data = dataInfered;
for ir = [1,2]
    for ix = 1:n
        [ps, f] =  pspectrum(data(:,ix,ir));
        fTrans = f/pi*xFreqB/2;
        psTrans = ps;
        
        maxSpec = max(ps);
        
        [~, loc, ~, ~] = findpeaks(log(psTrans),'MinPeakProminence',maxSpec);
        bw = 0.015;
        dataH = data(:,ix,ir);
        while ~isempty(loc)
            peak = fTrans(loc(end)); % ii(end) - vezme peak s nejvetsi prominenci
            dataH = bandstop(dataH,[peak-bw/2 peak+bw/2],xFreqB);
           
            loc = loc(1:end-1);
        end
        data(:,ix,ir) = dataH;
        
        subplot(4,n,2*n+ix)
        [ps, f] =  pspectrum(dataH);
        plot(f/pi*xFreqB/2,ps)
        set(gca, 'XScale', 'log')
        set(gca, 'YScale', 'log')
        grid on
        grid minor
    end
end

ir = 1;
for ix = 1:n
    subplot(4,n,3*n+ix)
    [ac, lag] = xcorr(data(:,ix,ir),'normalized');
    plot(lag,ac)
    xlim([-30 30])
    ylim([0.9 1.02])
end

% Moznost implementovat rychlejsi filtr - butterworth (butter)
% ...zkousel jsem, ale nevim jak presne funguje
%{
peak = fTrans(loc(end))/max(fTrans);
[b,a] = butter(3,[peak-bw/2 peak+bw/2],'stop');
dataH = filter(b,a,dataH);
%}

%% Check after bandstop

% Check signals
ir = 1;

figure
% 
for ix = 1:n    
    subplot(3,n,ix)
    dat = data(:,ix,ir);
    plot(dat)
    title(labs(:,ix,ir))
    
    subplot(3,n,n+ix)
    hold on
    [ps, f] =  pspectrum(dataInfered(:,ix,1));
    plot(f/pi*xFreqB/2,ps)
    
    [ps, f] =  pspectrum(dataInfered(:,ix,2));
    plot(f/pi*xFreqB/2,ps)
    legend('real','imaginary','Location','southwest')
    set(gca, 'XScale', 'log')
    set(gca, 'YScale', 'log')
    hold off
    
    
    subplot(3,n,2*n+ix)
    hold on
    [ps, f] =  pspectrum(dataInfered(:,ix,ir));
    plot(f/pi*xFreqB/2,ps)
    
    [ps, f] =  pspectrum(dat-mean(dat));
    plot(f/pi*xFreqB/2,ps)
    legend('before bandstop','after bandstop','Location','southwest')
    set(gca, 'XScale', 'log')
    set(gca, 'YScale', 'log')
    hold off
end 
sgtitle(xRunId) 

%% All at once
close all
ini = 15;
fin = 70;

setVideoRecording = false; %set video recording

for ir = [1,2]
    for dataIx = 1%:2:3
        for dataJx = dataIx+1%:2:4
            
            if (dataJx-dataIx)~=1
                continue
            end
            
            
            tau_0 = NaN(1,fin);
            tau_p = NaN(1,fin);
            tau_LSC = NaN(2,fin);
            
            title(['Run: ',xRunId])
            if (setVideoRecording == true)
                folder = ['..\h-runs\figures\',num2str(id),'\TffDep_detrending\'];
                if ~exist(folder, 'dir')
                    mkdir(folder)
                end
                pathVideoAVI = [folder,'data-',num2str(dataIx),'-',num2str(dataJx),'_ri-',num2str(ir),'_prominences.avi'];
                v = VideoWriter(pathVideoAVI);
                
                v.FrameRate = 2;
                v.Quality = 100;
                open(v);
            end
            
            signal1 = data(:,dataIx,ir);
            signal2 = data(:,dataJx,ir);
            
            for ix = ini:fin
                if ix ==41
                    breakPoint=0;
                end
                close
                figure
                set(gcf,'WindowState','fullscreen')
                set(gcf,'visible','off');
                tit = ['run: ',xRunId,', tff factor: ',num2str(ix)]
                sgtitle(tit) 
                
                % X1:
                %====
                [detrendedSignal1,prom1(ix),T0_1] = detrendMovMean(signal1,xFreqB*tff*ix);
                
                subplot(4,4,1)
                [ac1, lag ] = xcorr(detrendedSignal1);%,'normalized');
                lag = lag/xFreqB;
                max1 = max(ac1);
                ac1 = ac1/max1;
                plot(lag, ac1)
                xlabel('[s]')
                title(labs(:,dataIx,ir))
                
                subplot(4,4,5)
                plot(lag, ac1)
                xlabel('[s]')
                xlim([-100*tff, 100*tff])
                xline(T0_1/xFreqB)
                
                subplot(4,4,9)
                [ps, f] =  periodogram(detrendedSignal1);
                plot(f,ps)
                set(gca, 'XScale', 'log')
                set(gca, 'YScale', 'log')
                ylim([1e-27 1e-5])
                xlim([1e-5 5])
                
                subplot(4,4,13)
                plot(detrendedSignal1)
                xlim([1e1 1e3])
                
                
                % X2:
                %====
                [detrendedSignal2,prom2(ix),T0_2] = detrendMovMean(signal2,xFreqB*tff*ix);
                
                subplot(4,4,2)
                [ac2, lag ] = xcorr(detrendedSignal2);%,'normalized');
                lag = lag/xFreqB;
                max2 = max(ac2);
                ac2 = ac2/max2;
                plot(lag, ac2)
                xlabel('[s]')
                title(labs(:,dataJx,ir))
                
                subplot(4,4,6)
                plot(lag, ac2)
                xlim([-100*tff, 100*tff])
                xline(T0_2/xFreqB)
                xlabel('[s]')
                
                subplot(4,4,10)
                [ps, f] =  periodogram(detrendedSignal2);
                plot(f,ps)
                set(gca, 'XScale', 'log')
                set(gca, 'YScale', 'log')
                ylim([1e-27 1e-5])
                xlim([1e-5 5])
                
                subplot(4,4,14)
                plot(detrendedSignal2)
                xlim([1e1 1e3])
                
                
                % CC:
                %====
                subplot(4,4,[3,7])
                title('CC')
                [cc, lag] = xcorr(detrendedSignal1,detrendedSignal2,'normalized');
                lag = lag/xFreqB;
                [tau_p(ix),~] = taup(cc, xFreqB);
                [tau_0(ix),~,~] = tau0(cc,ac1,ac2,xFreqB);
                hold on
                plot(lag, cc)
                plot(lag, ac1)
                plot(lag, ac2)
                xlabel('[s]')      
                xlim([-20 20])
                hold off
                
                
                subplot(4,4,[11,15])
                hold on
                plot(lag, cc)
                plot(lag, ac1)
                plot(lag, ac2)
                xline(tau_p(ix),'-','\tau_p','Color','k','LabelHorizontalAlignment','left')
                xline(tau_0(ix),'-','\tau_0','Color','b','LabelHorizontalAlignment','right')
                xline(0,'--')
                yline(cc(round(length(cc)*0.5)), '-.','Color','b')
                
                xlabel('[s]')      
                xlim([-tff, tff])
                ylim([0.2 1.1])
                hold off
                 
                % tau(TFF_factor):
                %=================
                subplot(4,4,[4,8])
                hold on
                tau_LSC(1,ix) = T0_1/xFreqB;
                tau_LSC(2,ix) = T0_2/xFreqB;
                plot(1:fin, tau_LSC(1,:),'.-')
                plot(1:fin, tau_LSC(2,:),'.-')
                ylim([11.5 20])
                xlim([ini-1 fin+1])
                
                xlabel('TFF factor')
                legend('\tau_{LSC1}','\tau_{LSC2}','position',[0 0 .2 .2],'Location','northeast')
                hold off
                
                
                subplot(4,4,[12,16])
                hold on
                plot(1:fin, tau_0(:),'.-')
                plot(1:fin, tau_p(:),'.-')
                ylim([0.15 0.3])
                xlim([ini-1 fin+1])
                
                xlabel('TFF factor')
                legend('\tau_0','\tau_p','position',[0 0 .1 .1],'Location','east')
                hold off
                
                if (setVideoRecording == true)
                    frame = getframe(gcf);
                    writeVideo(v,frame);
                end
            end
            if (setVideoRecording == true)
                close(v);
            end
            
            [~,windowSizes(ir,dataIx)] = max(prom1);
            windowSizes(ir,dataIx) = (windowSizes(ir,dataIx))*tff*xFreqB;
            [data(:,dataIx,ir),~,T0_1] = detrendMovMean(signal1, windowSizes(ir,dataIx));
            T0(ir,dataIx) = T0_1/xFreqB;
            
            [~,windowSizes(ir,dataJx)] = max(prom2);
            windowSizes(ir,dataJx) = (windowSizes(ir,dataJx))*tff*xFreqB;
            [data(:,dataJx,ir),~,T0_2] = detrendMovMean(signal2, windowSizes(ir,dataJx));
            T0(ir,dataJx) = T0_2/xFreqB;
            
            cc = xcorr(data(:,dataIx,ir),data(:,dataJx,ir));
            ac1 = xcorr(data(:,dataIx,ir));
            ac2 = xcorr(data(:,dataJx,ir));
            [tau_p_fin(ir,round(dataIx/2)),~] = taup(cc, xFreqB);
            [tau_0_fin(ir,round(dataIx/2)),~,~] = tau0(cc,ac1,ac2,xFreqB);
        end
    end
end

%% Save all
%save([fileFolder,'data_afterDetrend.mat'])

%% Save after computation
tau_p = tau_p_fin;
tau_0 = tau_0_fin;
T0_mat = T0;
save([fileFolder,'characteristics.mat'],'T0_mat','tau_p','tau_0','L','nu','d')
%% Load
id = '083';
fileFolder = ['D:\Kasa\prace\Turbulence_new\h-runs\',id,'\'];
load([fileFolder,'characteristics.mat'])
%% Characteristic numbers - NOT COMPLETE - where should we mean values
for ir = [1 2]
    T0(ir) = mean(T0_mat(ir,:));    % second autocorr peak time
    f0(ir) = 1/T0(ir);              %
    Ref0(ir) = 2*L^2*f0(ir)/nu      % fluctuations frequency Reynolds
    
    Tp(ir) = tau_p(ir)/d;           %
    Up(ir) = 1/Tp(ir);              % plum velocity
    Rep(ir) = L*Up(ir)/nu;          % plumes velocity Reynolds
    
    U(ir) = d*tau_p(ir)/tau_0(ir)^2;
    V(ir) = d/tau_0(ir)*realsqrt(1-(tau_p(ir)/tau_0(ir))^2);
    
    Ueff(ir) = sqrt(U(ir)^2+V(ir)^2)
    %Ueff_(ir) = d/tau_0(ir)
    
    ReU(ir) = L*U(ir)/nu        % eliptic model velocity Reynolds
    ReV(ir) = L*V(ir)/nu        % eliptic model velocity Reynolds
    ReEff(ir) = L*Ueff(ir)/nu   % effective Reynolds
end

%%
clear
id = '083';
fileFolder = ['D:\Kasa\prace\Turbulence_new\h-runs\',id,'\']
load([fileFolder,'data_afterDetrend.mat'])
%% High-Low
%{
Outline:
    step
    windowSize
    eventInx = 1;
    - for inx = windowSize/2:step:(length(signal)-windSize/2)
        - vypocitej CC: [cc,lag] = xcorr(signal1,signal2,'normalized');
        [~, taup_arr(inx)]= taup(cc(length(cc)/2),xFreqB)

        - if CC(i)*CC(i-1)
            - eventWind[end+1] = i-1        
%}


windowSize = round(100*tff*xFreqB); 
if mod(windowSize,2)~=0         % must be even
    windowSize = windowSize+1;
end
halfWS = windowSize/2; % real window size is windowSize+1

step = round(windowSize/16); % 1/16th of window size ... arbitrary chosen

taup_arr = NaN(halfWS,n/2,2);
for ir = [1,2]
    for dataIx = 1%:2:3
        for dataJx = dataIx+1%:2:4
            if dataJx-dataIx == 1
                eventInx = 0;              
                signal1 = data(:,dataIx,ir);
                signal2 = data(:,dataJx,ir);
                
                for inx = halfWS+1:step:(length(signal1)-halfWS-step)
                    cc = xcorr( signal1(inx-halfWS:inx+halfWS),...
                                signal2(inx-halfWS:inx+halfWS),...
                                'normalized');
                    [~, taup_arr(inx,dataIx,ir)]= taup(cc,xFreqB); % taking parabolic tau_p
                    taup_arr(inx-step:inx,dataIx,ir) = taup_arr(inx,dataIx,ir);
                    
                    if inx>123974 && ir == 2 
                        a=0;
                    end
                    if (taup_arr(inx,dataIx,ir)*taup_arr(inx-step-1,dataIx,ir)<=0)
                        eventInx = eventInx+1;
                        eventWind(eventInx,dataIx,ir) = inx-step;
                        if (taup_arr(inx,dataIx,ir)*taup_arr(inx-step-1,dataIx,ir)==0)
                            disp('Hle, dej s pravdepodobnosti 0 nastal!')
                        end
                    end
                end
            end                
        end
    end
end
eventWind(eventWind==0)=NaN;
taup_arr(taup_arr==0)=NaN;
%% Plot
dataIx = 1;
ir = 1;

signal = data(:,dataIx,ir);
taup_signal = taup_arr(:,dataIx,ir);

figure

for ir = [1,2]
    signal = (-1)^(ir+1)*data(:,dataIx,ir);
    taup_signal = taup_arr(:,dataIx,ir);
    ax(ir)=subplot(2,1,ir)
    hold on
    plot(signal)
    plot(taup_signal)
    yline(0)
    for ix = 1:length(eventWind(:,dataIx,ir))
        if ~isnan(eventWind(ix,dataIx,ir))
            xline(eventWind(ix,dataIx,ir),'--')
        end
    end
    hold off
end
linkaxes([ax(1),ax(2)]);
