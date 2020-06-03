export interface PatientReception {
    ReceptionId: number;
    PatientId: number;
    DoctorId: number;
    HospitalId: number;
    Time: string;
    Duration: string;
    Date: string;
    DayOfWeek: string;
    Address: string;
    Purpose: string;
    Result: string;
    Price: number;
    Name: string;
    Data: string;
    Signature: string;
    IsPayed: boolean;
}