import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [patients, setPatients] = useState();

    const [patientName, setPatientName] = useState('');
    const [patientAge, setPatientAge] = useState('');

    const handlePatientNameChange = (event) => {
        setPatientName(event.target.value);
    };

    const handlePatientAgeChange = (event) => {
        setPatientAge(event.target.value);
    };

    const [diagnosesInputs, setDiagnosesInputs] = useState({});

    const handleDiagnosisInputChange = (patientId, value) => {
        setDiagnosesInputs({
            ...diagnosesInputs,
            [patientId]: value,
        });
    };

    const [selectedPatient, setSelectedPatient] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const openModal = async (patientId) => {
        const patient = await getPatientsHistory(patientId);

        setSelectedPatient(patient);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setSelectedPatient(null);
        setIsModalOpen(false);
    };


    useEffect(() => {
        populatePatientsData();
    }, []);

    const createPatientModule =
        <table>
            <tbody>
                <input
                    type="text"
                    id="name"
                    placeholder="Name"
                    value={patientName}
                    onChange={handlePatientNameChange} />
                <input
                    type="number"
                    id="age"
                    placeholder="Age"
                    value={patientAge}
                    onChange={handlePatientAgeChange} />
                <input type="button" value="Create Patient" onClick={createPatient} />
            </tbody>
        </table>;

    const usersModule = patients === undefined
        ? <p><em>Create a patient first</em></p>
        : <div>
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Age</th>
                        <th>Last diagnostics</th>
                        <th>Diagnosis</th>
                    </tr>
                </thead>
                <tbody>
                    {patients && patients.length && patients.map(patient =>
                        <tr key={patient.id}>
                            <td>{patient.name}</td>
                            <td>{patient.age}</td>
                            <td>{patient.diagnoses[0]?.date}</td>
                            <td>{patient.diagnoses[0]?.result}</td>
                            <td>
                                <input
                                    type="text"
                                    placeholder="New Diagnosis"
                                    value={diagnosesInputs[patient.id] || ''}
                                    onChange={(e) => handleDiagnosisInputChange(patient.id, e.target.value)}
                                />
                            </td>
                            <td>
                                <button onClick={() => addDiagnosis(patient.id)}>Add diagnosis</button>
                            </td>
                            <td>
                                <button onClick={() => openModal(patient.id)}>
                                    View Diagnoses
                                </button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

            {isModalOpen && selectedPatient && (
                <div className="modal">
                    <div className="modal-content">
                        <span className="close" onClick={closeModal}>
                            &times;
                        </span>
                        <h2>{selectedPatient.name}&apos;s Diagnoses</h2>
                        <table>
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Result</th>
                                </tr>
                            </thead>
                            <tbody>
                                {selectedPatient.diagnoses && selectedPatient.diagnoses.length && selectedPatient.diagnoses.map((diagnosis) => (
                                    <tr key={diagnosis.id}>
                                        <td>{diagnosis.date}</td>
                                        <td>{diagnosis.result}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}
        </div>;

    const addDiagnosis = async (patientId) => {
        const newDiagnosis = diagnosesInputs[patientId];
        if (!newDiagnosis) {
            alert('Please enter a diagnosis before submitting.');
            return;
        }

        const query = `
        mutation addDiagnosis($patientId: Int!, $result: String!) {
            addDiagnosis(patientId: $patientId, result: $result) {
                date
                result
            }
        }
    `;

        const variables = {
            patientId: parseInt(patientId),
            result: newDiagnosis,
        };

        const payload = JSON.stringify({ query, variables });

        try {
            const response = await fetch('/graphql', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: payload,
            });

            const result = await response.json();
            if (response.ok) {
                console.log('Diagnosis added:', result.data.addDiagnosis);
                populatePatientsData();
                setDiagnosesInputs({ ...diagnosesInputs, [patientId]: '' });
            } else {
                console.error('GraphQL error:', result.errors);
            }
        } catch (error) {
            console.error('Error adding diagnosis:', error);
        }
    };


    return (
        <div>
            <h1 id="tableLabel">Neurona Lab - Patient Diagnostics Management</h1>
            {createPatientModule}
            {usersModule}
        </div>
    );

    async function createPatient() {
        const query =`
            mutation addPatient($name: String!, $age: Int!) {
                addPatient(name: $name, age: $age) {
                    id
                    name
                    age
                }
            }`;

        const variables = {
            name: patientName,
            age: parseInt(patientAge),
        };

        const payload = JSON.stringify({
            query: query,
            variables: variables,
        });

        const response = await fetch(
            '/graphql',
            {
                method: 'POST',
                body: payload,
                headers: {
                    'Content-Type': 'application/json',
                    // 'Authorization': 'Apikey YOUR_API_KEY',
                },
            }
        );

        const result = await response.json();

        if (response.ok) {
            console.log('Created patient:', result.data.addPatient);
        } else {
            console.error('GraphQL error:', result.errors);
        }

        populatePatientsData()
    }

    async function populatePatientsData() {

        const query = JSON.stringify({
            query: `{
                patients {
                    id
                    name
                    age
                    diagnoses {
                        date
                        result
                    }
                }
            }`,
        });

        const response = await fetch(
            'graphql',
            {
                method: 'post',
                body: query,
                headers: {
                    'Content-Type': 'application/json',
                    //'Authorization': 'Apikey YOUR_API_KEY',
                },
            }
        );
        const data = await response.json();

        setPatients(data.data.patients);
    }

    async function getPatientsHistory(patientId) {
        const query = `
            query patient($id: Int!) {
                patient(id: $id) {
                    id
                    name
                    age
                    diagnoses {
                        date
                        result
                    }
                }
            }
        `;

        const variables = {
            id: parseInt(patientId),
        };

        const payload = JSON.stringify({
            query: query,
            variables: variables,
        });

        const response = await fetch(
            'graphql',
            {
                method: 'post',
                body: payload,
                headers: {
                    'Content-Type': 'application/json',
                    //'Authorization': 'Apikey YOUR_API_KEY',
                },
            }
        );
        const data = await response.json();

        return data.data.patient;
    }
}

export default App;