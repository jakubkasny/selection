function [tau_0,tau_0_parab,tau_0_error] = tau0(CC,AC1,AC2, xFreq)
% tau0 function for computing tau0 and parabolic tau0
% input:
%   CC              ... crosscorrelation function of signal 1 and 2
%   AC1             ... acutocerreltaion of signal 1
%   AC2             ... acutocerreltaion of signal 2
%   xFreq           ... sampling frequency
%   
% output:
%   tau_0           ... tau0 of signal
%   tau_0_parab     ... parabolic tau0 of signal
%   tau_0_error     ... difference between tau0 and parabolic tau0

try
    CC = cell2mat(CC);
catch
end

try
    AC1 = cell2mat(AC1);
catch    
end

try
    AC2 = cell2mat(AC2);
catch
end
    
%{
if (abs(min(CC))>abs(max(CC)))
    CC = -CC;
end
figure
hold on
lag = -(corrWndw-1):(corrWndw-1);
plot(lag, CC)
plot(lag,AC1)
plot(lag,AC2)
ylim([0.2, 1.1])
xline(0)
yline(yLvl)
legend('CC','AC1','AC2')
hold off

%}

corrWndw = round(length(CC)*0.5); % correlation window
yLvl = CC(corrWndw);

n = 4;
x = abs(AC1-yLvl);
[~, index] = sort(x);
x1 = sort(index(1:n));

x = abs(AC2-yLvl);
[~, index] = sort(x);
x2 = sort(index(1:n));
clear x

tau0arr1 = (x1 - corrWndw)/xFreq;
tau0arr2 = (x2 - corrWndw)/xFreq;

tau0mean1 = mean(abs(tau0arr1));
tau0mean2 = mean(abs(tau0arr2));

tau_0 = (tau0mean1+tau0mean2)*0.5;


%Real - parabolic:
[~, maxInx] = max(CC);
maxRange = maxInx-2:maxInx+2;
pCC = polyfit(maxRange-corrWndw, CC(maxRange)',2);

[~, maxInx] = max(AC1);
maxRange = maxInx-2:maxInx+2;
pAC1 = polyfit(maxRange-corrWndw, AC1(maxRange)',2);

[~, maxInx] = max(AC2);
maxRange = maxInx-2:maxInx+2;
pAC2 = polyfit(maxRange-corrWndw, AC2(maxRange)',2);

tau0arr1Par = (-pAC1(2)+realsqrt(pAC1(2)^2-4*pAC1(1)*(pAC1(3)-pCC(3))) )/(2*pAC1(1));

tau0arr2Par = (-pAC2(2)+realsqrt(pAC2(2)^2-4*pAC2(1)*(pAC2(3)-pCC(3))) )/(2*pAC2(1));

tau_0_parab = mean([tau0arr1Par,tau0arr2Par])/xFreq;

%error calculation:
tau_0_error = abs(tau_0-tau_0_parab);

%plot check:
%{
evCC = polyval(pCC,-20:80);
evAC1 = polyval(pAC1,-20:80);
evAC2 = polyval(pAC2,-20:80);

figure
hold on
plot(-20:80,evCC)
plot(-20:80,evAC1)
plot(-20:80,evAC2)
%ylim([0, 1.2])
xline(0)
yline(pCC(3))
legend('CC','AC1','AC2')
hold off
%}
end
