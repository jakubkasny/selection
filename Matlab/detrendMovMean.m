function [detrendedSignal, prom, T0] = detrendMovMean(signal, windowSize)
% detrendMovMean is a function for detrending signal using moving average
% input:
%   signal          ... data to be detrended
%   windowSize      ... size of window for moving average detrending
% output:
%   detrendedSignal ... detrended data
%   prom            ... prominence of 2nd peak
%   T0              ... LSC period in samples [-] 
%                       (divide by sampling frequency to obtains seconds)

signal = signal - mean(signal);

ma = movmean(signal, round(windowSize));
%{
figure(1)
hold on
plot(signal)
plot(ma)
%}
signal = signal - ma;

[AC,~] = xcorr(signal,'normalized');
%{
figure(2)
findpeaks(AC)
xlim([length(AC)/2-20 length(AC)/2+20])
%}
[~, loc, ~, p] = findpeaks(AC);

[~,ii] = sort(p);
prom = p(ii(end-1));

detrendedSignal = signal;

T0 = abs(loc(ii(end-1))-length(signal));

%close all
end
%{
figure
plot(AC)
xline(T0+length(signal))
%}
