<template>
  <v-dialog
    v-model="dialog"
    max-width="500px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'core.edit'"
      />
    </template>
    <v-card>
      <v-form v-model="form">
        <v-card-title>
          <span
            class="headline"
            v-t="'project.knowledgeBase.renameTopic'"
          ></span>
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
          <v-btn
            text
            v-t="'core.close'"
            @click.stop="dialog = false"
          />
          <v-spacer />
          <DeleteTopicDialog :topic="topic" />
          <v-btn
            text
            color="primary"
            type="submit"
            v-t="'core.save'"
            @click.stop="renameTopic"
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
import DeleteTopicDialog from './DeleteTopicDialog.vue'

@Component({
  components: {
    DeleteTopicDialog,
  }
})
export default class UpdateTopicDialog extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  @Prop(Topic)
  private readonly topic?: Topic

  private form: any = null
  private dialog: any = null
  private loading: boolean = false

  private topicName: string = ''

  private async created() {
    this.topicName = this.topic?.name || ''
  }

  private async renameTopic() {
    this.loading = true

    if (this.topic) {
      await this.knowledgeBaseModule.updateTopic({
        topicId: this.topic.id,
        name: this.topicName,
      })
    }

    this.loading = false
    this.dialog = false
  }
}
</script>
