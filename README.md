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
   - Open Unity Hub → Add project → select project folder.  
3. **Add Vuforia**  
   - Download Vuforia package: [link](https://drive.google.com/file/d/1wB97NFgZQPUIoY1IleKmjGoG44A81A0N/view?usp=sharing)  
   - In Unity: `Assets → Import Package → Custom Package...` → select the downloaded file and then → click **Import All**.
   - ⚠️ After that, if the import **does not add Vuforia correctly** (i.e., it doesn’t appear in *Window → Package Management → Package Manager → In Project*):

     1. Open **Window → Package Management → Package Manager**.  
     2. Click the **“+”** button (top left).  
     3. Select **“Add package from tarball...”**.  
     4. Navigate to `RVA/packages/` and choose the file  
        `com.ptc.vuforia.engine-11.4.4.tgz`.  
     5. Wait for Unity to import the package — it should now appear as **Vuforia Engine 11.4.4** in the Package Manager.  
4. **Use markers**  
   - Project includes Sun and Earth image markers.  
   - Print markers or use webcam to view AR.  
5. **Play**  
   - Activate webcam.  
   - Press Play in Unity.  

---

## Notes

- Vuforia package is **not in GitHub** due to file size.  
- Install manually via Unity as described above.  

---

## Project Structure

