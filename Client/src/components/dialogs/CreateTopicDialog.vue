<template>
  <v-dialog
    v-model="dialog"
    max-width="500px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.knowledgeBase.createTopic'"
      />
    </template>
    <v-card>
      <v-form v-model="form">
        <v-card-title>
          <span
            class="headline"
            v-t="'project.knowledgeBase.createTopic'"
          />
        </v-card-title>
        <v-card-text>
          <v-text-field
            v-model="topicName"
            :label="$t('core.name')"
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.close'"
          />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            @click.stop="createTopic"
            :disabled="topicName == ''"
            :loading="loading"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import KnowledgeBaseModule from '../../store/knowledgeBase'
import { getModule } from 'vuex-module-decorators'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Topic } from '../../models'
import i18n from '../../i18n'

@Component
export default class CreateTopicDialog extends Vue {
  private knowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private form: any = null
  private dialog: any = null
  private loading = false

  private topicName: string = ''

  private async createTopic() {
    this.loading = true

    await this.knowledgeBaseModule.createTopic(this.topicName)

    this.loading = false
    this.dialog = false
  }
}
</script>
