using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;


using System.Collections;

using System.ComponentModel;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.IO;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    class ViewData
    {
        public VMBenchmark vMBenchmark = new VMBenchmark();
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnCollectionChanged(NotifyCollectionChangedAction ev)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public bool CollectionChangedAfterSave { get; set; }

        public string ErrorMessage
        {
            get; set;
        }

        public void AddVMtime(VMf vMf, VMGird vMGird)
        {
            try
            {


                vMBenchmark.AddVMtime(vMf, vMGird);
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
                /*OnPropertyChanged("");*/
                CollectionChangedAfterSave = true;
                OnPropertyChanged("CollectionChangedAfterSave");
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "Add failed: " + ex.Message;
            }
        }
        public void AddVMAccuracy(VMf vMf, VMGird vMGird)
        {
            try
            {

                vMBenchmark.AddVMAccuracy(vMf, vMGird);
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
                /*OnPropertyChanged("");*/
                CollectionChangedAfterSave = true;
                OnPropertyChanged("CollectionChangedAfterSave");
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "Add failed: " + ex.Message;
            }
        }
        public void Save(string filename)
        {
            Directory.SetCurrentDirectory("..\\..\\..\\");
            FileStream FS = null;

            try
            {
                if (File.Exists(filename))
                    FS = File.OpenWrite(filename);
                else
                    FS = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(FS, vMBenchmark);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "Save failed:" + ex.Message;
            }
            finally
            {
                if (FS != null)
                    FS.Close();
                CollectionChangedAfterSave = false;
                OnPropertyChanged("CollectionChangedAfterSave");
            }
        }

        public bool Load(string filename)
        {
            Directory.SetCurrentDirectory("..\\..\\..\\");
            FileStream FS = null;

            try
            {
                FS = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                vMBenchmark = (VMBenchmark)binaryFormatter.Deserialize(FS);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "Load: " + ex.Message;
            }
            finally
            {
                FS.Close();
                CollectionChangedAfterSave = true;
                OnPropertyChanged("CollectionChangedAfterSave");
            }

            return true;
        }

    }
}