function [tau_p,tau_p_parab] = taup(signal, xFreq)
% taup function for computing taup and parabolic taup
% input:
%   signal          ... autocorrelation or crosscorrelation function
%   xFreq           ... sampling frequency
% output:
%   tau_p           ... taup of signal
%   tau_p_parab     ... parabolic taup of signal

try
    signal = cell2mat(signal);
catch
end

[~, maxInx] = max(signal);
Tau_p_index = maxInx - round(length(signal)*0.5);
tau_p = Tau_p_index/xFreq;

% parabolic:
% - fit n values around maximum
% - from the coefficients compute the max x-position
maxRange = maxInx-2:maxInx+2;
p = polyfit(maxRange-round(length(signal)*0.5),signal(maxRange)',2);

maxCCparab = -p(2)/(2*p(1));
tau_p_parab = maxCCparab/xFreq;

% numerical solution:
%{
evalRange = maxInx-1:0.01:maxInx+1
evaluation = polyval(p,evalRange,S,mu);
[~, maxCCparab] = max(evaluation);
maxCCparab = evalRange(maxCCparab)- round(length(signal)*0.5);
%}
%plot check:
%{
evaluation = polyval(p,(maxInx-10:0.01:maxInx+10),S,mu);

figure
hold on
plot(signal,-numel(signal)/2:numel(signal)/2)
plot((maxInx-10:0.01:maxInx+10),evaluation)
hold off
%}
%plot check:
%{
maxRange = maxInx-2:maxInx+2;
[p,S,mu] = polyfit(maxRange-round(length(signal)*0.5),signal(maxRange)',2);
evaluation = polyval(p,(maxInx-10:0.01:maxInx+10),S,mu);
figure
hold on
plot(-floor(numel(signal)/2):floor(numel(signal)/2),signal)
plot((maxInx-10:0.01:maxInx+10),evaluation)
hold off
%}
end
