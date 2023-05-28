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
%% Auto interfering frequencies finder

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
       
    end
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
tic
close all
ini = 15;
fin = 70;

for ir = [1,2]
    for dataIx = 1%:2:3
        for dataJx = dataIx+1%:2:4
            
            signal1 = data(:,dataIx,ir);
            signal2 = data(:,dataJx,ir);
            
            for ix = ini:fin
                if ix ==41
                    breakPoint=0;
                end
                ix
                % X1:
                %====
                [~,prom1(ix),~] = detrendMovMean(signal1,xFreqB*tff*ix);
                
                % X2:
                %====
                [~,prom2(ix),~] = detrendMovMean(signal2,xFreqB*tff*ix);
                
            end
            
            [~,windowSizes(ir,dataIx)] = max(prom1);
            windowSizes(ir,dataIx) = (windowSizes(ir,dataIx))*tff*xFreqB;
            [data(:,dataIx,ir),~,T0_1] = detrendMovMean(signal1, windowSizes(ir,dataIx));
            T0(ir,dataIx) = T0_1/xFreqB;
            
            [~,windowSizes(ir,dataJx)] = max(prom2);
            windowSizes(ir,dataJx) = (windowSizes(ir,dataJx))*tff*xFreqB;
            [data(:,dataJx,ir),~,T0_2] = detrendMovMean(signal2, windowSizes(ir,dataJx));
            T0(ir,dataJx) = T0_2/xFreqB;
            
            cc = xcorr(data(:,dataIx,ir),data(:,dataJx,ir),'normalized');
            ac1 = xcorr(data(:,dataIx,ir),'normalized');
            ac2 = xcorr(data(:,dataJx,ir),'normalized');
            [tau_p_fin(ir,round(dataIx/2)),tau_p_fin_parab(ir,round(dataIx/2))] = taup(cc, xFreqB);
            [tau_0_fin(ir,round(dataIx/2)),~,~] = tau0(cc,ac1,ac2,xFreqB);
        end
    end
end
toc
%% Save all
save([fileFolder,'data_afterDetrend.mat'])

%% Save numbers for computing results
tau_p = tau_p_fin_parab;
tau_p_numeric = tau_p_fin;
tau_0 = tau_0_fin;
T0_mat = T0;
save([fileFolder,'characteristics.mat'],'T0_mat','tau_p','tau_p_numeric','tau_0','L','nu','d')
%% Load
id = '083';
fileFolder = ['D:\Kasa\prace\Turbulence_new\h-runs\',id,'\'];
load([fileFolder,'characteristics.mat'])

%% Events - wind reversals


if (soucin == 0)
    disp('Hle, dej s pravdepodobnosti 0 nastal!')
end


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
