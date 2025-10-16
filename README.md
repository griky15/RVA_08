# RVA - Virtual and Augmented Reality

Unity 6.2 project for Virtual and Augmented Reality Course - MEIC with **Vuforia** framework.

---

## Requirements

- Unity 6.2  
- Git (optional)  
- Internet to download packages

---

## Members

- Guilherme Cruz
- Rodrigo Pinheiro
- Tomás Monteiro

---

## Setup

1. **Install Unity 6.2** via Unity Hub.  
2. **Get the project**  
   - Clone the repository or download the ZIP.  
   - Open Unity Hub → Add project → select project folder. If prompted, select version 6.2 (abbreviated).
   - Run the project. It should appear a warning about Vuforia, click 'Continue'. Then, an error should appear, click 'Ignore'. We will tackle Vuforia abscense in step number 3.
   - Now that the project is open, a warning about deprecated packages should appear. Click 'Dismiss' or 'Dismiss forever'.
3. **Add Vuforia**  
   - Download Vuforia .unitypackage: [link](https://drive.google.com/file/d/1wB97NFgZQPUIoY1IleKmjGoG44A81A0N/view?usp=sharing)
   - In Unity: `Assets → Import Package → Custom Package...` → select the downloaded file and then → click **Import All**.
   - After that, download Vuforia .tgz package [link](https://drive.google.com/file/d/1LtTjOWkaUAiHhP03P2akzZJmfjE6MIIO/view?usp=sharing) and follow the steps: 

     1. Open **Window → Package Management → Package Manager**.  
     2. Click the **“+”** button (top left).  
     3. Select **“Add package from tarball...”**.  
     4. Navigate to your downloaded .tgz file and choose it:
        `com.ptc.vuforia.engine-11.4.4.tgz`.  
     5. Wait for Unity to import the package — it should now appear as **Vuforia Engine 11.4.4** in the Package Manager.
6. **Finally**  
   - You will see a blank scene on your Unity. It's normal. Go to File → Open Scene and navigate to \RVA_08-master\Assets\Scenes\SolarSystem.unity. Choose the file.
5. **Use markers**  
   - Project includes Sun and Earth image markers.  
   - Print markers or use webcam to view AR. 
6. **Play**  
   - Activate webcam.  
   - Press Play in Unity.
   - Display the markers to the camera, et voilà!

---

## Notes

- Vuforia package is **not in GitHub** due to file size.  
- Install manually via Unity as described above.  

---

## Project Structure

