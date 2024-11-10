import SwiftUI
import AVFoundation

struct KickboxingTimerView: View {
    @State private var roundDuration: Int = 120 // Round süresi (saniye)
    @State private var restDuration: Int = 30 // Dinlenme süresi (saniye)
    @State private var currentRound: Int = 1
    @State private var isRunning: Bool = false
    @State private var timeLeft: Int = 120
    @State private var totalRounds: Int = 5
    @State private var isResting: Bool = false
    
    // Sesli uyarılar için player
    var soundPlayer: AVAudioPlayer?
    
    var body: some View {
        ZStack {
            Color.black.edgesIgnoringSafeArea(.all) // Arka plan siyah
            
            VStack(spacing: 40) {
                Text(isResting ? "Dinlenme Arası" : "Round \(currentRound)")
                    .font(.largeTitle)
                    .foregroundColor(.white)
                
                Text("\(timeLeft) saniye")
                    .font(.system(size: 50))
                    .foregroundColor(timeLeft <= 10 ? .red : .white) // Son 10 saniye kırmızı
                
                ZStack {
                    Circle()
                        .stroke(lineWidth: 10)
                        .opacity(0.3)
                        .foregroundColor(.white)
                    
                    Circle()
                        .trim(from: 0.0, to: CGFloat(timeLeft) / CGFloat(isResting ? restDuration : roundDuration))
                        .stroke(timeLeft <= 10 ? Color.red : Color.green, lineWidth: 10)
                        .rotationEffect(.degrees(-90))
                        .animation(.linear(duration: 1.0), value: timeLeft)
                }
                .frame(width: 150, height: 150)
                
                HStack {
                    Button(action: startTimer) {
                        Text("Başlat")
                            .font(.title)
                            .foregroundColor(.white)
                            .frame(width: 120, height: 50)
                            .background(Color.green)
                            .cornerRadius(10)
                    }
                    .opacity(isRunning ? 0 : 1)
                    
                    Button(action: stopTimer) {
                        Text("Durdur")
                            .font(.title)
                            .foregroundColor(.white)
                            .frame(width: 120, height: 50)
                            .background(Color.red)
                            .cornerRadius(10)
                    }
                    .opacity(isRunning ? 1 : 0)
                }
                
                Stepper("Round Sayısı: \(totalRounds)", value: $totalRounds, in: 1...20)
                    .foregroundColor(.white)
                    .padding()
                
                Stepper("Round Süresi: \(roundDuration) sn", value: $roundDuration, in: 30...300, step: 30)
                    .foregroundColor(.white)
                    .padding()
            }
        }
    }
    
    func startTimer() {
        isRunning = true
        timeLeft = roundDuration
        playSound() // İlk başlatıldığında sesli uyarı
        Timer.scheduledTimer(withTimeInterval: 1.0, repeats: true) { timer in
            if timeLeft > 0 {
                timeLeft -= 1
            } else {
                if isResting {
                    isResting = false
                    currentRound += 1
                    timeLeft = roundDuration
                } else {
                    if currentRound < totalRounds {
                        isResting = true
                        timeLeft = restDuration
                    } else {
                        isRunning = false
                        timer.invalidate()
                    }
                }
                
                if timeLeft == 10 {
                    playCountdownWarningSound()
                }
                
                playSound()
            }
        }
    }
    
    func stopTimer() {
        isRunning = false
    }
    
    func playSound() {
        // Ses dosyasını çalma işlevi (sesli uyarı eklenebilir)
    }
    
    func playCountdownWarningSound() {
        // Son 10 saniye uyarı sesi
    }
}
