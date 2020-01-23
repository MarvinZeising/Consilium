<template>

  <v-menu
    v-model="model.model"
    :close-on-content-click="false"
    :activator="model.element"
    offset-x
  >
    <v-card min-width="350px">
      <v-toolbar
        :color="model.event.color"
        prominent
      >
        <v-toolbar-title>
          {{ userModule.getUser.formatTime(model.event.start, 'YYYY-MM-DD HH:mm') }}
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <!--//TODO: check write permission -->
        <UpdateShiftDialog :shift="model.event.shift" />
        <v-btn
          icon
          @click="model.model = false"
        >
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar>
      <v-card-text>
        <span>
          Lot's of exciting stuff!
        </span>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <CreateApplicationDialog :shift="getShift" />
      </v-card-actions>
    </v-card>
  </v-menu>

</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import UpdateShiftDialog from './UpdateShiftDialog.vue'
import CreateApplicationDialog from './CreateApplicationDialog.vue'

@Component({
  components: {
    UpdateShiftDialog,
    CreateApplicationDialog,
  },
})
export default class CreateTaskDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)

  @Prop(Object)
  private readonly model?: {
    model: any,
    element: any,
    event: any,
  }

}
</script>
