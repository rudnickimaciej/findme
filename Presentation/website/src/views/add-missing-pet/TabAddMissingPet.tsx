// ** React Imports
import { useState, forwardRef, ElementType, ChangeEvent } from 'react'
import axios from 'axios';


// ** MUI Imports
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import Radio from '@mui/material/Radio'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import TextField from '@mui/material/TextField'
import FormLabel from '@mui/material/FormLabel'
import InputLabel from '@mui/material/InputLabel'
import RadioGroup from '@mui/material/RadioGroup'
import CardContent from '@mui/material/CardContent'
import FormControl from '@mui/material/FormControl'
import OutlinedInput from '@mui/material/OutlinedInput'
import FormControlLabel from '@mui/material/FormControlLabel'
import { styled } from '@mui/material/styles'
import Button, { ButtonProps } from '@mui/material/Button'
import Typography from '@mui/material/Typography'

// ** Third Party Imports
import DatePicker from 'react-datepicker'

// ** Styled Components
import DatePickerWrapper from 'src/@core/styles/libs/react-datepicker'

const CustomInput = forwardRef((props, ref) => {
  return <TextField inputRef={ref} label='Missing date' fullWidth {...props} />
})

const ImgStyled = styled('img')(({ theme }) => ({
  width: 120,
  height: 120,
  marginRight: theme.spacing(6.25),
  borderRadius: theme.shape.borderRadius,
  cursor: 'pointer', 
  transition: 'transform 0.2s ease-in-out', // Add a transition for a smooth effect
  '&:hover': {
    transform: 'scale(1.1)', // Scale the image on hover
  },
}))

const ButtonStyled = styled(Button)<ButtonProps & { component?: ElementType; htmlFor?: string }>(({ theme }) => ({
  [theme.breakpoints.down('sm')]: {
    width: '100%',
    textAlign: 'center'
  }
}))

const ResetButtonStyled = styled(Button)<ButtonProps>(({ theme }) => ({
  marginLeft: theme.spacing(4.5),
  [theme.breakpoints.down('sm')]: {
    width: '100%',
    marginLeft: 0,
    textAlign: 'center',
    marginTop: theme.spacing(4)
  }
}))

const TabAddMissingPet = () => {
  // ** State
  const [missingDate, setMissingDate] = useState<Date | null | undefined>(null)
  const [name, setName] = useState<string>();
  const [description, setDescription] = useState<string>();
  const [gender, setGender] = useState<number>();
  const [breadId, setBreadId] = useState<string>();
  const [photoUrls, setPhotoUrls] = useState<string[]>(['/images/avatars/add-photo.png']);
  
  // const [imgSrc, setImgSrc] = useState<string>('/images/avatars/1.png')

  const onImageClick = (index: number) => () => {
    // Trigger the hidden file input for the corresponding image index
    document.getElementById(`account-settings-upload-image-${index}`)?.click();
  };

  const onChange = (index: number) => (file: ChangeEvent) => {
    const reader = new FileReader();
    const { files } = file.target as HTMLInputElement;
    if (files && files.length !== 0) {
      reader.onload = () => {
        const updatedPhotos = [...photoUrls];
        updatedPhotos[index] = reader.result as string;
        setPhotoUrls(updatedPhotos);
      };

      reader.readAsDataURL(files[0]);
    }
  };

  const uploadPhotosToCloudStorage = async () => {
    const formData = new FormData();
    
    for (let i = 0; i < photoUrls.length; i++) {
      formData.append('files', photoUrls[i]);
    }
    console.log('FormData content:');
    for (const pair of formData.entries()) {
      console.log(pair[0] + ', ' + pair[1]);
    }

    try {
      const response = await axios.post('http://localhost:5073/api/files', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
  
      if (response.status === 200) {
        const result = response.data;
        console.log('Upload successful:', result);
        sendFormDataToApi(result);
      } else {
        console.error('Upload failed:', response.statusText);
      }
    } catch (error) {
      console.error('Error during upload:', error);
    }
  };
  
  const sendFormDataToApi = async (photoUrls: any) => {
    // Send a request to your API endpoint with the form data
    // (e.g., using the Fetch API or Axios)
    const formData = {
      name: name,
      description: description,
      missingDate: missingDate,
      gender: gender,
      breadId: breadId,
      photoUrls: photoUrls
    }
    const response = await fetch('http://localhost:5193/api/missingpets', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(formData),
    });
  
    if (!response.ok) {
      // Handle non-successful response (e.g., throw an error)
      throw new Error('Failed to submit form');
    }
  
    // Parse and return the API response
    return response.json();
  };

  return (
    <CardContent>
      <form>
        <Grid container spacing={7}>
         
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label='Pet name'
              placeholder='Your Pet name'
              defaultValue=''
              onChange = {(value: any)=>setName(value)}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <DatePickerWrapper>
              <DatePicker
                selected={missingDate}
                showYearDropdown
                showMonthDropdown
                id='account-settings-date'
                placeholderText='MM-DD-YYYY hh:mm'
                customInput={<CustomInput />}
                onChange={(date: Date) => setMissingDate(date)}
              />
            </DatePickerWrapper>
          </Grid>
    
          <Grid item xs={12} sm={6}>
            <FormControl fullWidth>
              <InputLabel>Bread</InputLabel>
              <Select label='Bread' defaultValue='5E1DB0DE-B154-4300-8948-769CFCAE9EE9' onChange={(value:any) => setBreadId(value.target.value)}>
                <MenuItem value='5E1DB0DE-B154-4300-8948-769CFCAE9EE9'>Mieszaniec</MenuItem>
                <MenuItem value='67089718-7B54-4400-A2A5-A21604688294'>Labrador</MenuItem>
                <MenuItem value='A91BAF21-29D1-4589-A303-8E9CD8999E46'>Unknown</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} sm={6}>
            <FormControl>
              <FormLabel sx={{ fontSize: '0.875rem' }}>Gender</FormLabel>
              <RadioGroup row defaultValue='1' aria-label='gender' name='account-settings-info-radio'  onChange={(event: React.ChangeEvent<HTMLInputElement>) => setGender(Number(event.target.value))}>
                <FormControlLabel value='1' label='Male' control={<Radio />} />
                <FormControlLabel value='0' label='Female' control={<Radio />} />
              </RadioGroup>
            </FormControl>
          </Grid>
          <Grid item xs={12} sx={{ marginTop: 4.8 }}>
            <TextField
              fullWidth
              multiline
              label='Description'
              minRows={2}
              placeholder='Describe you pet'
              defaultValue=''
              onChange= {(value:any)=>setDescription(value)}
            />
          </Grid>
          <Grid item xs={12} sx={{ marginTop: 4.8, marginBottom: 3 }}>
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            {/* Map over the array of photos */}
            {photoUrls.map((photo, index) => (
              <ImgStyled key={index} src={photo} alt={`Profile Pic ${index + 1}`} onClick={onImageClick(index)} />
            ))}
            
            {/* Render the hidden file inputs */}
            {photoUrls.map((_, index) => (
              <input
                key={index}
                hidden
                type='file'
                onChange={onChange(index)}
                accept='image/png, image/jpeg'
                id={`account-settings-upload-image-${index}`}
              />
            ))}


              <Box>
                <Typography variant='body2' sx={{ marginTop: 5 }}>
                  Adding photos increases the chance to find your pet.
                </Typography>
              </Box>
            </Box>
          </Grid>
          <Grid item xs={12}>
            <Button variant='contained' sx={{ marginRight: 3.5 } } onClick={() => uploadPhotosToCloudStorage()}>
              Send
            </Button>
            <Button type='reset' variant='outlined' color='secondary' onClick={() => setMissingDate(null)}>
              Reset
            </Button>
          </Grid>
        </Grid>
      </form>
    </CardContent>
  )
}

export default TabAddMissingPet
