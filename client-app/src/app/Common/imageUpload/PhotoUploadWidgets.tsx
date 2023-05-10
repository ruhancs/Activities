import { Button, ButtonGroup, Grid, GridColumn, Header, Image } from "semantic-ui-react";
import PhotoDropzone from "./PhotoDropZone";
import { useEffect, useState } from "react";
import PhotoCropper from "./PhotoCropper";
// import { Cropper } from "react-cropper";

interface IProps {
    loading: boolean;
    uploadPhoto: (file: Blob) => void
}

export default function PhotoUploadWidgets({loading,uploadPhoto}: IProps) {
    const [files, setFiles] = useState<any>([]);
    const [cropper, setCropper] = useState<Cropper>();

    function onCrop() {
        if(cropper) {
            cropper.getCroppedCanvas().toBlob(blob => uploadPhoto(blob!))
        }
    }

    useEffect(() => {
        return () => {
            // apagar o preview da imagem da memoria
            files.forEach((file:any) => URL.revokeObjectURL(file.preview))
        }
    },[files])

    return (
        <Grid>
            <GridColumn width={4}>
                <Header sub color="teal" content="Step 1 - Add Photo" />
                <PhotoDropzone setFiles={setFiles} />
            </GridColumn>
            <GridColumn width={1} />
            <GridColumn width={4}>
                <Header sub color="teal" content="Step 2 - Resize Image" />
                {files && files.length > 0 && (
                    <PhotoCropper setCropper={setCropper} imagePreview={files[0].preview} />
                )}
            </GridColumn>
            <GridColumn width={1} />
            <GridColumn width={4}>
                <Header sub color="teal" content="Step 3 - Preview & Upload" />
                {files && files.length > 0 &&
                    <>
                        <div className="img-preview" style={{minHeight: 200, overflow: 'hidden'}} />
                        <ButtonGroup widths={2}>
                            <Button loading={loading} onClick={onCrop} positive icon='check' />
                            <Button disabled={loading} onClick={() => setFiles([])}  icon='close' />
                        </ButtonGroup>
                    </>
                }
            </GridColumn>
        </Grid>
    )
}